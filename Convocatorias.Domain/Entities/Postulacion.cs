using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Entities
{
    public sealed class Postulacion
    {
        public Guid Id { get; private set; }
        public Guid ConvocatoriaId { get; private set; }
        public Guid CandidatoId { get; private set; }
        public DateTime FechaPostulacion { get; private set; }
        public EstadoPostulacion Estado { get; private set; }

        private readonly List<Documento> _documentos = new ();
        public IReadOnlyCollection<Documento> Documentos => _documentos;

        public Postulacion(Guid convocatoriaId, Guid candidatoId)
        {
            if (convocatoriaId == Guid.Empty)
                throw new ArgumentException("Convocatoria inválida");

            if (candidatoId == Guid.Empty)
                throw new ArgumentException("Candidato inválido");

            Id = Guid.NewGuid();
            ConvocatoriaId = convocatoriaId;
            CandidatoId = candidatoId;
            FechaPostulacion = DateTime.UtcNow;
            Estado = EstadoPostulacion.Pendiente; // Estado inicial
        }
        public void AgregarDocumento(Documento documento)
        {
            if(documento == null)
                throw new ArgumentNullException(nameof(documento));
            
            if(Estado != EstadoPostulacion.Pendiente)
                throw new InvalidOperationException("Solo se pueden agregar documentos a postulaciones en estado 'Pendiente'.");
            
            _documentos.Add(documento);
        }

        public void CambiarEstado(EstadoPostulacion nuevoEstado)
        {
            Estado = nuevoEstado;
        }
    }
}
