using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class ConvPeriodoRepository(ConvocatoriasDbContext context) : BaseRepository<ConvocatoriaPeriodo>(context), IConvPeriodoRepository
    {
        public async Task AddAsync(ConvocatoriaPeriodo convocatoriaPeriodo, CancellationToken ct = default)
        {
            await DbSet.AddAsync(convocatoriaPeriodo, ct);
        }

        public async Task DesactivarOtrosPeriodos(Guid convocatoriaId, CancellationToken ct = default)
        {
            await DbSet
                .Where(cp => cp.ConvocatoriaId == convocatoriaId && cp.EsActual)
                .ExecuteUpdateAsync(
                    cp => cp.SetProperty(p => p.EsActual, false), ct);
        }

        public async Task<IEnumerable<ConvocatoriaPeriodo>> GetByConvocatoriaIdAsync(Guid convocatoriaId, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(cp => cp.ConvocatoriaId == convocatoriaId)
                .OrderByDescending(cp => cp.AsignadoEn)
                .ToListAsync(ct);
        }

        public async Task<Guid> GetPeriodoIdVigenteAsync(CancellationToken ct = default)
        {
            var hoy = DateTime.UtcNow;

            var periodoVigenteId = await DbSet
                .Include(cp => cp.Periodo)
                .Where(cp => cp.EsActual && cp.Periodo.FechaInicio <= hoy && cp.Periodo.FechaFin >= hoy)
                .FirstOrDefaultAsync(ct);

            return periodoVigenteId?.Id ?? Guid.Empty;
        }

    }
}
