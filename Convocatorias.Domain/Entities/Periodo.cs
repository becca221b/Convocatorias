using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    public sealed class Periodo
    {
        public Guid Id { get; private set; }
        public string Orden { get; private set; }
        public int Cuatrimestre { get; private set; }
        public int Anio { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        private Periodo() { }
        public Periodo(string orden, int cuatrimestre, int anio, DateTime fechaInicio, DateTime fechaFin)
        {
            if (string.IsNullOrWhiteSpace(orden)) {
                throw new ArgumentException("El orden del período no puede estar vacío");
            }
            if (cuatrimestre < 1 || cuatrimestre > 2) {
                    throw new ArgumentException("El cuatrimestre debe ser 1 o 2");
            }
            if (fechaInicio >= fechaFin) {
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin");
            }
            Id = Guid.NewGuid();
            Orden = orden;
            Cuatrimestre = cuatrimestre;
            Anio = anio;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public bool EstaVigente()
        {
            var ahora = DateTime.UtcNow;
            return FechaInicio <= ahora && ahora <= FechaFin;
        }
    }
}
