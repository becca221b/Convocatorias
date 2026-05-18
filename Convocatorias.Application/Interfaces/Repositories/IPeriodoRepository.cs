using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IPeriodoRepository
    {
        Task<Periodo> GetByIdAsync(Guid id);
    }
}
