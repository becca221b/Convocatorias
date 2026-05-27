using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Convocatorias.Application.UseCases.Postularse;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.Interfaces;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Tests
{
    public class PostularseTest
    {
        [Fact]
        public async Task Postular_Deberia_Crear_Postulacion_Correctamente()
        {
            // Arrange
            var convocatoriaId = Guid.NewGuid();
            var candidato = new Candidato("Juan", "Pérez", "juan.perez@example.com");
            // Añadir documentación requerida
            candidato.AgregarEducacion(new Educacion("Licenciatura", 2010, PosgradoStatus.NoAplica, string.Empty, "Grado"));
            candidato.AgregarExperienciaDocente(new ExperienciaDocente(5, Nivel.Universitario, "UBA", "Profesor", DateTime.UtcNow.AddYears(-5), DateTime.UtcNow));
            var candidatoId = candidato.Id;

            var periodo = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1));
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodo);

            

            var postulacionRepo = new Mock<IPostulacionRepository>();
            Postulacion? postulacionCapturada = null;
            postulacionRepo
                .Setup(r => r.PostulacionExistsAsync(convocatoriaId, candidatoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            postulacionRepo
                .Setup(r => r.AddAsync(It.IsAny<Postulacion>(), It.IsAny<CancellationToken>()))
                .Callback<Postulacion, CancellationToken>((p, ct) => postulacionCapturada = p)
                .Returns(Task.CompletedTask);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            
            periodoRepo
                .Setup(r => r.GetVigenteAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var candidatoRepo = new Mock<ICandidatoRepository>();
            candidatoRepo
                .Setup(r => r.GetByIdAsync(candidatoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidato);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var useCase = new PostularseUseCase(postulacionRepo.Object, convocatoriaRepo.Object, candidatoRepo.Object, periodoRepo.Object, uow.Object);
            var request = new PostularseRequest { ConvocatoriaId = convocatoriaId, CandidatoId = candidatoId };

            // Act
            var response = await useCase.Postular(request);

            // Assert
            Assert.NotNull(postulacionCapturada);
            Assert.Equal(convocatoriaId, postulacionCapturada!.ConvocatoriaId);
            Assert.Equal(candidatoId, postulacionCapturada.CandidatoId);

            postulacionRepo.Verify(r => r.PostulacionExistsAsync(convocatoriaId, candidatoId, It.IsAny<CancellationToken>()), Times.Once);
            postulacionRepo.Verify(r => r.AddAsync(It.IsAny<Postulacion>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(postulacionCapturada!.Id, response.PostulacionId);
            Assert.Equal("Postulación realizada con éxito", response.Mensaje);
        }

        [Fact]
        public async Task Postular_Deberia_Lanzar_Si_Convocatoria_No_Existe()
        {
            var convocatoriaId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.PostulacionExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Convocatoria?)null);

            var periodoRepo = new Mock<IPeriodoRepository>();
            var candidatoRepo = new Mock<ICandidatoRepository>();
            var uow = new Mock<IUnitOfWork>();

            var useCase = new PostularseUseCase(postulacionRepo.Object, convocatoriaRepo.Object, candidatoRepo.Object, periodoRepo.Object, uow.Object);
            var request = new PostularseRequest { ConvocatoriaId = convocatoriaId, CandidatoId = candidatoId };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Postular(request));
        }

        [Fact]
        public async Task Postular_Deberia_Lanzar_Si_Convocatoria_Esta_Cerrada()
        {
            var convocatoriaId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();

            var periodo = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1));
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, periodo);
            convocatoria.CerrarConvocatoria();

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.PostulacionExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var periodoRepo = new Mock<IPeriodoRepository>();
            var candidatoRepo = new Mock<ICandidatoRepository>();
            var uow = new Mock<IUnitOfWork>();

            var useCase = new PostularseUseCase(postulacionRepo.Object, convocatoriaRepo.Object, candidatoRepo.Object, periodoRepo.Object, uow.Object);
            var request = new PostularseRequest { ConvocatoriaId = convocatoriaId, CandidatoId = candidatoId };

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Postular(request));
        }

        [Fact]
        public async Task Postular_Deberia_Lanzar_Si_Candidato_Ya_Se_Ha_Postulado()
        {
            var convocatoriaId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.PostulacionExistsAsync(convocatoriaId, candidatoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1))));

            var periodoRepo = new Mock<IPeriodoRepository>();
            var candidatoRepo = new Mock<ICandidatoRepository>();
            var uow = new Mock<IUnitOfWork>();

            var useCase = new PostularseUseCase(postulacionRepo.Object, convocatoriaRepo.Object, candidatoRepo.Object, periodoRepo.Object, uow.Object);
            var request = new PostularseRequest { ConvocatoriaId = convocatoriaId, CandidatoId = candidatoId };

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Postular(request));
        }

        [Fact]
        public async Task Postular_Deberia_Lanzar_Si_Candidato_No_Cumple_Requisitos()
        {
            var convocatoriaId = Guid.NewGuid();
            var candidato = new Candidato("Ana", "García", "ana.garcia@example.com"); // sin educación ni experiencia
            var candidatoId = candidato.Id;

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.PostulacionExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1))));

            var periodoRepo = new Mock<IPeriodoRepository>();
            var candidatoRepo = new Mock<ICandidatoRepository>();
            candidatoRepo
                .Setup(r => r.GetByIdAsync(candidatoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidato);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new PostularseUseCase(postulacionRepo.Object, convocatoriaRepo.Object, candidatoRepo.Object, periodoRepo.Object, uow.Object);
            var request = new PostularseRequest { ConvocatoriaId = convocatoriaId, CandidatoId = candidatoId };

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Postular(request));
        }
    }
}
