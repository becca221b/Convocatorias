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

        

        //Test para caso de uso: no crear postulacion para una convocatoria que no está disponible
    }
}
