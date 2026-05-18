using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IConvPeriodoRepository
    {
        Task AddAsync(ConvocatoriaPeriodo convocatoriaPeriodo);
         Task<IEnumerable<ConvocatoriaPeriodo>> GetByConvocatoriaIdAsync(Guid convocatoriaId);
         Task UpdateAsync(ConvocatoriaPeriodo convocatoriaPeriodo);
         Task<Guid> ObtenerPeriodoVigente();
         Task DesactivarOtrosPeriodos();
    }
}
