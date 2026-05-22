using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class CandidatoRepository(ConvocatoriasDbContext context) : BaseRepository<Candidato>(context), ICandidatoRepository
    {
        public async Task AddAsync(Candidato candidato, CancellationToken ct = default)
        {
            await DbSet.AddAsync(candidato, ct);
        }

        public Task<List<Candidato>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Candidato?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await DbSet
                .Include(c => c.Educaciones)
                .Include(c => c.ExperienciasDocente)
                .Include(c => c.ExperienciasInvExt)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<Candidato?> GetByIdSinDetallesAsync(Guid id, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task UpdateAsync(Candidato candidato, CancellationToken ct = default)
        {
            DbSet.Update(candidato);
            await Task.CompletedTask;
        }
    }
}
