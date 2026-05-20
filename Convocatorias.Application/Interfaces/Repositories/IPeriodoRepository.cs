using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IPeriodoRepository
    {
        Task<Periodo> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Periodo>> GetAllAsync();
        Task AddAsync(Periodo periodo);
    }
}
