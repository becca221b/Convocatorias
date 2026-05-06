using Convocatorias.Domain.Entities;

namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IConvocatoriaRepository
    {
        Task<Convocatoria> GetByIdAsync(Guid id);
        Task<bool> EstaDisponibleAsync(Guid id);
        Task<Periodo> GetPeriodoAsync(Guid convocatoriaId);
    }
}
