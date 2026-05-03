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
        Estado = nuevoEstado;
    }
}
}
