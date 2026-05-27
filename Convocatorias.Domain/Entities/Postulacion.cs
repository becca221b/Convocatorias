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

        public Postulacion() { }

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
    

        public void CambiarEstado(EstadoPostulacion nuevoEstado)
        {
            if (Estado == nuevoEstado)
                throw new InvalidOperationException("La postulacion ya se encuentra en el estado especificado.");

            // Lógica de validación para transiciones de estado
            if (Estado == EstadoPostulacion.Aprobada || Estado == EstadoPostulacion.Rechazada)
                throw new InvalidOperationException("No se puede cambiar el estado de una postulacion que ya ha sido aprobada o rechazada.");
            if (nuevoEstado == EstadoPostulacion.Pendiente)
                throw new InvalidOperationException("No se puede volver al estado pendiente.");
            if (nuevoEstado == EstadoPostulacion.Revision && Estado != EstadoPostulacion.Pendiente)
                throw new InvalidOperationException("Solo se puede pasar a revisión desde el estado pendiente.");
            if((nuevoEstado == EstadoPostulacion.Aprobada || nuevoEstado == EstadoPostulacion.Rechazada) && Estado != EstadoPostulacion.Revision)
                throw new InvalidOperationException("Solo se puede aprobar o rechazar una postulacion que está en revisión.");
            
            Estado = nuevoEstado;
        }
    }
}
