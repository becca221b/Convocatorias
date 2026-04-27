using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Tests
{
    public class ConvocatoriaTest
    {
        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Fechafin_es_menor()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(-1)));
        }

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Asignatura_Esta_Vacia()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1, "", "Presencial", DateTime.Now, DateTime.Now.AddDays(10)));
        }

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Modalidad_Esta_Vacia()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1, "Matemáticas", "", DateTime.Now, DateTime.Now.AddDays(10)));
        }

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Sede_Facultad_Carrera_No_Validos()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(0, 1, 1, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(10)));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 0, 1, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(10)));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 0, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(10)));
        }

        [Fact]
        public void Status_Deberia_Ser_Activa_Al_Crear_Convocatoria()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(10));
            Assert.Equal(Status.Activa, convocatoria.Status);

        }

        // Agrega más pruebas unitarias según sea necesario para cubrir otros métodos y casos de uso
        [Fact]
        public void CerrarConvocatoria_Deberia_Cambiar_Status_A_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now, DateTime.Now.AddDays(10));
            convocatoria.CerrarConvocatoria();
            Assert.Equal(Status.Cerrada, convocatoria.Status);
        }
        
        [Fact]
        public void EstaDisponible_Deberia_Devolver_True_Si_Esta_Activa_Y_Dentro_De_Periodo()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            Assert.True(convocatoria.EstaDisponible());
        }

        [Fact]
        public void EstaDisponible_Deberia_Devolver_False_Si_Esta_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            convocatoria.CerrarConvocatoria();
            Assert.False(convocatoria.EstaDisponible());
        }

        [Fact]
        public void ReabrirConvocatoria_Deberia_Cambiar_Fechas_Si_Esta_Activa()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            convocatoria.ReabrirConvocatoria(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
            Assert.True(convocatoria.EstaDisponible());
        }

        [Fact]
        public void ReabrirConvocatoria_Deberia_Lanzar_Error_Si_Esta_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", "Presencial", DateTime.UtcNow, DateTime.Now.AddDays(15));
            convocatoria.CerrarConvocatoria();
            Assert.Throws<InvalidOperationException>(() => convocatoria.ReabrirConvocatoria(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10)));
        }
         

    }
}
