using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;


namespace Convocatorias.Domain.Tests
{
    public class PostulacionTest
    {
        [Fact]
        public void Deberia_Crear_Postulacion_Con_Estado_Pendiente()
        {
            var postulacion = new Postulacion(Guid.NewGuid(), Guid.NewGuid());
            Assert.Equal(EstadoPostulacion.Pendiente, postulacion.Estado);
        }

        [Fact]
        public void AgregarDocumento_Deberia_Agregar_Documento_Si_Estado_Pendiente()
        {
            var postulacion = new Postulacion(Guid.NewGuid(), Guid.NewGuid());
            var documento = new Documento(TipoArea.ExperienciaDocente, 1, "cv", "ruta/al/archivo");
            postulacion.AgregarDocumento(documento);
            Assert.Equal(EstadoPostulacion.Pendiente, postulacion.Estado);
            Assert.Contains(documento, postulacion.Documentos);
        }

        //Test para caso de uso: no crear postulacion para una convocatoria que no está disponible
    }
}
