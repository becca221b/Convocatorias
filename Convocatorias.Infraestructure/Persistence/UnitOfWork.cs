
namespace Convocatorias.Infraestructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ConvocatoriasDbContext _context;
        public UnitOfWork(ConvocatoriasDbContext context)
        {
            _context = context;
        }
        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
