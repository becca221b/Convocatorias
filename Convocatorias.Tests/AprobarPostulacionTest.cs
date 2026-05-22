using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Convocatorias.Application.UseCases.AprobarPostulaciones;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.Interfaces;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Tests
{
    public class AprobarPostulacionTest
    {
        [Fact]
        public async Task Aprobar_Deberia_Aprobar_Postulacion_Y_Cerrar_Convocatoria()
        {
            // Arrange
            var convocatoriaId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();

            var postulacion = new Postulacion(convocatoriaId, candidatoId);
            postulacion.CambiarEstado(EstadoPostulacion.Revision); // preparar estado válido para aprobar

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.GetByIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postulacion);
            postulacionRepo
                .Setup(r => r.AddAsync(It.IsAny<Postulacion>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var useCase = new AprobarPostulacionUseCase(postulacionRepo.Object, convocatoriaRepo.Object, uow.Object);
            var request = new AprobarPostulacionRequest { PostulacionId = postulacion.Id };

            // Act
            var response = await useCase.Aprobar(request);

            // Assert
            Assert.Equal(postulacion.Id, response.PostulacionId);
            Assert.Equal(EstadoPostulacion.Aprobada.ToString(), response.Estado);
            Assert.Equal("Matemáticas", response.ConvocatoriaName);

            postulacionRepo.Verify(r => r.AddAsync(It.Is<Postulacion>(p => p.Id == postulacion.Id), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.False(convocatoria.estaAbierta());
        }

        [Fact]
        public async Task Aprobar_Deberia_Lanzar_Si_Postulacion_No_Existe()
        {
            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Postulacion?)null);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            var uow = new Mock<IUnitOfWork>();

            var useCase = new AprobarPostulacionUseCase(postulacionRepo.Object, convocatoriaRepo.Object, uow.Object);
            var request = new AprobarPostulacionRequest { PostulacionId = Guid.NewGuid() };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Aprobar(request));
        }

        [Fact]
        public async Task Aprobar_Deberia_Lanzar_Si_Convocatoria_No_Existe()
        {
            var convocatoriaId = Guid.NewGuid();
            var postulacion = new Postulacion(convocatoriaId, Guid.NewGuid());
            postulacion.CambiarEstado(EstadoPostulacion.Revision);

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.GetByIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postulacion);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Convocatoria?)null);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AprobarPostulacionUseCase(postulacionRepo.Object, convocatoriaRepo.Object, uow.Object);
            var request = new AprobarPostulacionRequest { PostulacionId = postulacion.Id };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Aprobar(request));
        }

        [Fact]
        public async Task Aprobar_Deberia_Lanzar_Si_Convocatoria_Esta_Cerrada()
        {
            var convocatoriaId = Guid.NewGuid();
            var postulacion = new Postulacion(convocatoriaId, Guid.NewGuid());
            postulacion.CambiarEstado(EstadoPostulacion.Revision);

            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.CerrarConvocatoria();

            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.GetByIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postulacion);

            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);

            var uow = new Mock<IUnitOfWork>();

            var useCase = new AprobarPostulacionUseCase(postulacionRepo.Object, convocatoriaRepo.Object, uow.Object);
            var request = new AprobarPostulacionRequest { PostulacionId = postulacion.Id };

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Aprobar(request));
        }

        [Fact]
        public async Task No_Deberia_Aprobar_Postulacion_si_ya_esta_aprobada_o_rechazada()
        {
            var convocatoriaId = Guid.NewGuid();
            var postulacion = new Postulacion(convocatoriaId, Guid.NewGuid());
            postulacion.CambiarEstado(EstadoPostulacion.Revision);
            postulacion.CambiarEstado(EstadoPostulacion.Aprobada);
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            var postulacionRepo = new Mock<IPostulacionRepository>();
            postulacionRepo
                .Setup(r => r.GetByIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postulacion);
            var convocatoriaRepo = new Mock<IConvocatoriaRepository>();
            convocatoriaRepo
                .Setup(r => r.GetByIdAsync(convocatoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(convocatoria);
            var uow = new Mock<IUnitOfWork>();
            var useCase = new AprobarPostulacionUseCase(postulacionRepo.Object, convocatoriaRepo.Object, uow.Object);
            var request = new AprobarPostulacionRequest { PostulacionId = postulacion.Id };
            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Aprobar(request));
        }
    }
}
