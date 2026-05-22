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
        public void Deberia_cambiar_el_estado_de_la_Postulacion()
        {
            var postulacion = new Postulacion(Guid.NewGuid(), Guid.NewGuid());
            postulacion.CambiarEstado(EstadoPostulacion.Revision);
            Assert.Equal(EstadoPostulacion.Revision, postulacion.Estado);
        }

        [Fact]
        public void Deberia_cambiar_el_estado_de_la_Postulacion_a_Aprobada()
        {
            var postulacion = new Postulacion(Guid.NewGuid(), Guid.NewGuid());
            postulacion.CambiarEstado(EstadoPostulacion.Revision);
            postulacion.CambiarEstado(EstadoPostulacion.Aprobada);
            Assert.Equal(EstadoPostulacion.Aprobada, postulacion.Estado);
        }

       
        //Test para caso de uso: no crear postulacion para una convocatoria que no está disponible
    }
}
