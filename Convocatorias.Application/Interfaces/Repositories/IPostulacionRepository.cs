using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IPostulacionRepository
    {
        Task<Postulacion?> GetByIdAsync(Guid postulacionId, CancellationToken ct = default);
        Task<bool> PostulacionExistsAsync(Guid convocatoriaId, Guid candidatoId, CancellationToken ct = default);
        Task AddAsync(Postulacion postulacion, CancellationToken ct = default);

        Task UpdateAsync(Postulacion postulacion, CancellationToken ct = default);
        Task DeleteAsync(Guid postulacionId, CancellationToken ct = default);

    }
}
