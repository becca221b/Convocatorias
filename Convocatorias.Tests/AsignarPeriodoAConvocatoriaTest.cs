using Moq;
using Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.Interfaces;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Tests
{
    public class AsignarPeriodoAConvocatoriaTest
    {
        [Fact]
        public async Task Asignar_Deberia_Asignar_Periodo_Correctamente()
        {
            // Arrange
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            convPeriodoRepo.Setup(r => r.DesactivarOtrosPeriodos(convocatoriaId)).Returns(Task.CompletedTask);
            convPeriodoRepo.Setup(r => r.AddAsync(It.IsAny<ConvocatoriaPeriodo>())).Returns(Task.CompletedTask);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);

            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            // Act
            var response = await useCase.Asignar(request);

            // Assert
            Assert.Equal(convocatoriaId, response.ConvocatoriaId);
            Assert.Equal(periodoId, response.PeriodoId);
            Assert.Equal(periodo.FechaInicio, response.FechaInicio);
            Assert.Equal(periodo.FechaFin, response.FechaFin);
            Assert.Equal($"{periodo.Orden} Convocatoria {periodo.Cuatrimestre} Cuatrimestre {periodo.Anio}", response.PeriodoNombre);

            convPeriodoRepo.Verify(r => r.DesactivarOtrosPeriodos(convocatoriaId), Times.Once);
            convPeriodoRepo.Verify(r => r.AddAsync(It.Is<ConvocatoriaPeriodo>(cp => cp.ConvocatoriaId == convocatoriaId && cp.PeriodoId == periodoId)), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // El periodo asignado debe quedar como actual en la convocatoria
            Assert.Equal(periodoId, convocatoria.ObtenerPeriodoActual());
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Convocatoria_No_Existe()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Convocatoria?)null);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Periodo?)null);

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Convocatoria_Esta_Cerrada()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.CerrarConvocatoria();

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Periodo_No_Existe()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Periodo?)null);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Periodo_No_Esta_Vigente()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-10),
                fechaFin: DateTime.UtcNow.AddDays(-5) // ya finalizado
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Convocatoria_Tiene_Mismo_Periodo()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.AgregarPeriodo(ConvocatoriaPeriodo.Crear(convocatoriaId, periodoId)); // ya asignado

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Hay_Periodo_Actual_Vigente_En_Convocatoria()
        {
            var convocatoriaId = Guid.NewGuid();
            var actualPeriodoId = Guid.NewGuid();
            var nuevoPeriodoId = Guid.NewGuid();

            var periodoActual = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var nuevoPeriodo = new Periodo(
                orden: 2,
                cuatrimestre: Cuatrimestre.Segundo,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            // periodo actual en la convocatoria
            convocatoria.AgregarPeriodo(ConvocatoriaPeriodo.Crear(convocatoriaId, actualPeriodoId));

            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            // cuando se consulta el periodo actual devolvemos uno que está vigente
            periodoRepo
                .Setup(r => r.GetByIdAsync(actualPeriodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoActual);

            // el nuevo periodo también existe y es vigente (aun así debería bloquear por periodo actual vigente)
            periodoRepo
                .Setup(r => r.GetByIdAsync(nuevoPeriodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(nuevoPeriodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = nuevoPeriodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Desactivar_Otros_Periodos_Vigentes()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId1 = Guid.NewGuid();
            var periodoId2 = Guid.NewGuid();
            var periodo1 = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-10),
                fechaFin: DateTime.UtcNow.AddDays(-6)
            );
            var periodo2 = new Periodo(
                orden: 2,
                cuatrimestre: Cuatrimestre.Segundo,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.AgregarPeriodo(ConvocatoriaPeriodo.Crear(convocatoriaId, periodoId1)); // periodo vigente actual
            var convPeriodoRepo = new Mock<IConvPeriodoRepository>();
            convPeriodoRepo.Setup(r => r.DesactivarOtrosPeriodos(convocatoriaId)).Returns(Task.CompletedTask);
            convPeriodoRepo.Setup(r => r.AddAsync(It.IsAny<ConvocatoriaPeriodo>())).Returns(Task.CompletedTask);
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);
            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo1);
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo2);
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var useCase = new AsignarPeriodo(convPeriodoRepo.Object, convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId2 };

            var response = await useCase.Asignar(request);

            Assert.NotNull(response);
            convPeriodoRepo.Verify(r => r.DesactivarOtrosPeriodos(convocatoriaId), Times.Once);
            // El periodo asignado debe quedar como actual en la convocatoria
            Assert.Equal(periodoId2, convocatoria.ObtenerPeriodoActual());
            //Ver que todos los periodos relacionados con la convocatoria excepto el nuevo periodo asignado queden desactivados
            Assert.All(convocatoria.Periodos, cp =>
            {
                if (cp.PeriodoId != periodoId2)
                {
                    Assert.False(cp.EsActual);
                }
                else
                {
                    Assert.True(cp.EsActual);
                }
            });





        }

    }
}
