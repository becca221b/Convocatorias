using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Convocatorias.Infraestructure.Repositories
{
    internal abstract class BaseRepository<TEntity>(ConvocatoriasDbContext context) where TEntity : class
    {
        protected readonly ConvocatoriasDbContext Context = context;
        protected DbSet<TEntity> DbSet => Context.Set<TEntity>();
    }
}
