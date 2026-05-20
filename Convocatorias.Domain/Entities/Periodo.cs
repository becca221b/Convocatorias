
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Entities
{
    public sealed class Periodo
    {
        public Guid Id { get; private set; }
        public int Orden { get; private set; }
        public Cuatrimestre Cuatrimestre { get; private set; }
        public int Anio { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }

        private readonly List<ConvocatoriaPeriodo> _convocatorias = [];

        public IReadOnlyCollection<ConvocatoriaPeriodo> Convocatorias
            => _convocatorias.AsReadOnly();

        private Periodo() { }
        public Periodo(int orden, Cuatrimestre cuatrimestre, int anio, DateTime fechaInicio, DateTime fechaFin)
        {
            if (orden <= 0) {
                throw new ArgumentException("El orden del período debe ser un número positivo");
            }
            
            if(anio < 2000 || anio > DateTime.UtcNow.Year + 1) {
                throw new ArgumentException("Año inválido");
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

        public bool EstaVigente(DateTime fechaActual)
        {           
            return FechaInicio <= fechaActual && fechaActual <= FechaFin && fechaActual.Year == Anio;
        }
    }
}
