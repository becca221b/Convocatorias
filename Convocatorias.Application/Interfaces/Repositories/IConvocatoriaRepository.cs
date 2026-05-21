using Convocatorias.Domain.Entities;

namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IConvocatoriaRepository
    {      
            Task<Convocatoria?> GetByIdAsync(Guid id, CancellationToken ct= default);
            Task<List<Convocatoria>> GetAllAsync(CancellationToken ct= default);
            Task AddAsync(Convocatoria convocatoria, CancellationToken ct= default);
            Task UpdateAsync(Convocatoria convocatoria, CancellationToken ct= default);
            Task DeleteAsync(Guid id, CancellationToken ct= default);
            Task<IReadOnlyCollection<Convocatoria>> GetAllAbiertasAsync(CancellationToken ct= default);
    }
}
