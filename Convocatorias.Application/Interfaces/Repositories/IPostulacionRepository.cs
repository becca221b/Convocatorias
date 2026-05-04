using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IPostulacionRepository
    {
        Task<bool> PostulacionExistsAsync(Guid convocatoriaId, Guid candidatoId);
        Task AddAsync(Postulacion postulacion);
    }
}
