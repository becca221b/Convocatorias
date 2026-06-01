
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class ConvocatoriaRepository(ConvocatoriasDbContext context) : BaseRepository<Convocatoria>(context), IConvocatoriaRepository
    {
        public async Task AddAsync(Convocatoria convocatoria, CancellationToken ct = default)
        {
            await DbSet.AddAsync(convocatoria, ct);
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Convocatoria>> GetAllAbiertasAsync(CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(c => c.Status == Status.Abierta)
                .OrderBy(c => c.Periodos.Max(p => p.AsignadoEn))
                .ToListAsync(ct);
        }

        public async Task<List<Convocatoria>> GetAllAsync(CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Convocatoria?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await DbSet
                .Include(c => c.Periodos)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task UpdateAsync(Convocatoria convocatoria, CancellationToken ct = default)
        {
            DbSet.Update(convocatoria);
            return Task.CompletedTask;
        }
        public async Task<List<Convocatoria>> GetAbiertasAsync(CancellationToken ct = default)
        {
            return await DbSet
                .Where(c => c.Status == Status.Abierta)
                .OrderBy(c => c.Periodos.Max(p => p.AsignadoEn))
                .ToListAsync(ct);
        }
    }
}
