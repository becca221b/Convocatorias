using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Tests
{
    public class ConvocatoriaTest
    {
        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Fechafin_es_menor()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid()));
        }

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Asignatura_Esta_Vacia()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1,"", Modalidad.Presencial, Guid.NewGuid()));
        }

       

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Sede_Facultad_Carrera_No_Validos()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(0, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid()));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 0, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid()));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 0, "Matemáticas", Modalidad.Presencial, Guid.NewGuid()));
        }

        [Fact]
        public void Status_Deberia_Ser_Activa_Al_Crear_Convocatoria()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            Assert.Equal(Status.Abierta, convocatoria.Status);

        }

        // Agrega más pruebas unitarias según sea necesario para cubrir otros métodos y casos de uso
        [Fact]
        public void CerrarConvocatoria_Deberia_Cambiar_Status_A_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.CerrarConvocatoria();
            Assert.Equal(Status.Cerrada, convocatoria.Status);
        }
        
        
        [Fact]
        public void EstaDisponible_Deberia_Devolver_False_Si_Esta_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            convocatoria.CerrarConvocatoria();
            Assert.False(convocatoria.ValidarAbierta());
        }

        [Fact]
        public void ReabrirConvocatoria_Deberia_Cambiar_Fechas_Si_Esta_Activa()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, Guid.NewGuid());
            var periodoViejo = convocatoria.Periodos.FirstOrDefault();
            convocatoria.ModificarPeriodo(Guid.NewGuid());
            Assert.True(convocatoria.ValidarAbierta());
            Assert.False(periodoViejo.EsActual);
           
        }

        
         

    }
}
