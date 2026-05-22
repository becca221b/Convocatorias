using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class PostulacionRepository(ConvocatoriasDbContext context) : BaseRepository<Postulacion>(context), IPostulacionRepository
    {
        public async Task AddAsync(Postulacion postulacion, CancellationToken ct = default)
        {
            await DbSet.AddAsync(postulacion, ct);
        }

        public Task DeleteAsync(Guid postulacionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Postulacion?> GetByIdAsync(Guid postulacionId, CancellationToken ct = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(p => p.Id == postulacionId, ct);
        }

        public async Task<bool> PostulacionExistsAsync(Guid convocatoriaId, Guid candidatoId, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .AnyAsync(p => p.ConvocatoriaId == convocatoriaId 
                && p.CandidatoId == candidatoId, ct);
        }

        public Task UpdateAsync(Postulacion postulacion, CancellationToken ct = default)
        {
            DbSet.Update(postulacion);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyCollection<Postulacion>> GetPostulacionesByConvocatoriaIdAsync(Guid convocatoriaId, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(p => p.ConvocatoriaId == convocatoriaId)
                .OrderByDescending(p => p.FechaPostulacion)    
                .ToListAsync(ct);
        }
    }
}
