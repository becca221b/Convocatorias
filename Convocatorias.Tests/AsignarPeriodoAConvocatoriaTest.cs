using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.Interfaces;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;   

namespace Convocatorias.Tests
{
    public class AsignarPeriodoAConvocatoriaTest
    {
        private static void SetPrivateId(object entity, Guid id)
        {
            var prop = entity.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            prop!.SetValue(entity, id);
        }

        [Fact]
        public async Task Asignar_Deberia_Asignar_Periodo_Correctamente()
        {
            // Arrange
            var convocatoriaId = Guid.NewGuid();

            var periodoViejo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddMonths(-2),
                fechaFin: DateTime.UtcNow.AddMonths(-1)
            );

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );
            var periodoId = periodo.Id;

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodoViejo);
            SetPrivateId(convocatoria, convocatoriaId);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            Convocatoria? capturedConv = null;
            convocatoriaRepo
                .Setup(r => r.UpdateAsync(It.IsAny<Convocatoria>(), It.IsAny<CancellationToken>()))
                .Callback<Convocatoria, CancellationToken>((c, ct) => capturedConv = c)
                .Returns(Task.CompletedTask);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            // Act
            var response = await useCase.Asignar(request);

            // Assert - respuesta
            Assert.Equal(convocatoriaId, response.ConvocatoriaId);
            Assert.Equal(periodo.FechaInicio, response.FechaInicio);
            Assert.Equal(periodo.FechaFin, response.FechaFin);
            Assert.Equal($"{periodo.Orden} Convocatoria {periodo.Cuatrimestre} Cuatrimestre {periodo.Anio}", response.PeriodoNombre);

            // Assert - persistencia y UoW
            convocatoriaRepo.Verify(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()), Times.Once);
            convocatoriaRepo.Verify(r => r.UpdateAsync(It.IsAny<Convocatoria>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Assert - estado del agregado pasado a UpdateAsync
            Assert.NotNull(capturedConv);
            Assert.Equal(periodoId, capturedConv!.ObtenerPeriodoActual());
            Assert.Contains(capturedConv.Periodos, p => p.PeriodoId == periodoId && p.EsActual);
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
            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Convocatoria_Esta_Cerrada()
        {
            var convocatoriaId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-1),
                fechaFin: DateTime.UtcNow.AddDays(1)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodo);
            SetPrivateId(convocatoria, convocatoriaId);
            convocatoria.CerrarConvocatoria();

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodo.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodo.Id };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Periodo_No_Existe()
        {
            var convocatoriaId = Guid.NewGuid();
            var periodoId = Guid.NewGuid();

            var periodo = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1));
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodo);
            SetPrivateId(convocatoria, convocatoriaId);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Periodo?)null);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Periodo_No_Esta_Vigente()
        {
            var convocatoriaId = Guid.NewGuid();

            var periodo = new Periodo(
                orden: 1,
                cuatrimestre: Cuatrimestre.Primer,
                anio: DateTime.UtcNow.Year,
                fechaInicio: DateTime.UtcNow.AddDays(-10),
                fechaFin: DateTime.UtcNow.AddDays(-5)
            );

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodo);
            SetPrivateId(convocatoria, convocatoriaId);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(periodo.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = periodo.Id };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }

       
        [Fact]
        public async Task Asignar_Deberia_Lanzar_Si_Hay_Periodo_Actual_Vigente_En_Convocatoria()
        {
            var convocatoriaId = Guid.NewGuid();
            var actualPeriodo = new Periodo(
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

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, actualPeriodo);
            SetPrivateId(convocatoria, convocatoriaId);
            

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            periodoRepo
                .Setup(r => r.GetByIdAsync(actualPeriodo.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(actualPeriodo);
            periodoRepo
                .Setup(r => r.GetByIdAsync(nuevoPeriodo.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(nuevoPeriodo);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AsignarPeriodo(convocatoriaRepo.Object, periodoRepo.Object, uow.Object);
            var request = new AsignarPeriodoRequest { ConvocatoriaId = convocatoriaId, PeriodoId = nuevoPeriodo.Id };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Asignar(request));
        }
    }
}
