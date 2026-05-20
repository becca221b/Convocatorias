using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;

namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class PeriodoRepository : IPeriodoRepository
    {
        private readonly ConvocatoriasDbContext dbContext;

        public PeriodoRepository(ConvocatoriasDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Periodo periodo)
        {
            await dbContext.Periodos.AddAsync(periodo);
        }

        public async Task<IReadOnlyList<Periodo>> GetAllAsync()
        {
            return await dbContext.Periodos
                .AsNoTracking()
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Cuatrimestre)
                .ToListAsync();
        }

        public async Task<Periodo> GetByIdAsync(Guid id)
        {
            return await dbContext.Periodos.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
