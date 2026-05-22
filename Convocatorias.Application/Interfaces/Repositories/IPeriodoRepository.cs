using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IPeriodoRepository
    {
        Task<Periodo?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Periodo>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Periodo periodo, CancellationToken ct = default);
        Task<Guid> GetVigenteIdAsync(CancellationToken ct = default);
    }
}
