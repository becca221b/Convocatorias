using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Tests
{
    public class ConvocatoriaTest
    {
        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Asignatura_Esta_Vacia()
        {
            var periodoInicial = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 1,"", Modalidad.Presencial, periodoInicial));
        }

       

        [Fact]
        public void No_Deberia_Crear_Convocatoria_Si_Sede_Facultad_Carrera_No_Validos()
        {
            Assert.Throws<ArgumentException>(() => new Convocatoria(0, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1))));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 0, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1))));
            Assert.Throws<ArgumentException>(() => new Convocatoria(1, 1, 0, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1))));
        }

        [Fact]
        public void Status_Deberia_Ser_Abierta_Al_Crear_Convocatoria()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1)));
            Assert.Equal(Status.Abierta, convocatoria.Status);

        }

        // Agrega más pruebas unitarias según sea necesario para cubrir otros métodos y casos de uso
        [Fact]
        public void CerrarConvocatoria_Deberia_Cambiar_Status_A_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1)));
            convocatoria.CerrarConvocatoria();
            Assert.Equal(Status.Cerrada, convocatoria.Status);
        }
        
        
        [Fact]
        public void ValidarAbierta_Deberia_Devolver_False_Si_Esta_Cerrada()
        {
            var convocatoria = new Convocatoria(1, 1, 1, "Matemáticas", Modalidad.Presencial, new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(1)));
            convocatoria.CerrarConvocatoria();
            Assert.False(convocatoria.estaAbierta());
        }

        




    }
}
