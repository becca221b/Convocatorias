
namespace Convocatorias.Domain.Entities
{
    public sealed class ConvocatoriaPeriodo
    {
        public Guid Id { get; private set; }
        public Guid ConvocatoriaId { get; private set; }
        public Guid PeriodoId { get; private set; }

        public bool EsActual { get; private set; }
        public DateTime AsignadoEn { get; private set; }

        public Convocatoria Convocatoria { get; private set; } = null!;

        public Periodo Periodo { get; private set; } = null!;
        public ConvocatoriaPeriodo() { }
        
        public static ConvocatoriaPeriodo Crear(Guid convocatoriaId, Guid periodoId)
        {
            return new ConvocatoriaPeriodo
            {
                Id = Guid.NewGuid(),
                ConvocatoriaId = convocatoriaId,
                PeriodoId = periodoId,
                EsActual = true,
                AsignadoEn = DateTime.UtcNow
            };
        }

        public void MarcarComoActual()
        {
            EsActual = true;
            AsignadoEn = DateTime.UtcNow;
        }

        public void MarcarComoNoActual()
        {
            EsActual = false;
        }

    }
}
