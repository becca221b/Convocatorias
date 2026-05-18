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

        public Task AddAsync(Periodo periodo)
        {
            
        }

        public Task<IEnumerable<Periodo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Periodo> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(dbContext.Periodos.FirstOrDefault(p => p.Id == id));
        }
    }
}
