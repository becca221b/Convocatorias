using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Convocatorias.Infraestructure.Repositories
{
    internal sealed class PeriodoRepository(ConvocatoriasDbContext context) : BaseRepository<Periodo>(context) , IPeriodoRepository
    {

        public Task AddAsync(Periodo periodo, CancellationToken ct = default)
        {
            return DbSet.AddAsync(periodo, ct).AsTask();
        }

        public async Task<IReadOnlyList<Periodo>> GetAllAsync(CancellationToken ct = default)
        {
            return await DbSet
                 .AsNoTracking()
                 .OrderByDescending(p => p.Anio)
                 .ThenByDescending(p => p.Cuatrimestre)
                 .ToListAsync(ct);
        }

     
        public async Task<Periodo?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Periodo> GetVigenteAsync(DateTime fecha, CancellationToken ct = default)
        {
            var periodo = await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.FechaInicio <= fecha && p.FechaFin >= fecha, ct);

            if (periodo is null)
                throw new InvalidOperationException("No existe un periodo vigente para la fecha especificada.");

            return periodo;
        }

        public Task<Guid> GetVigenteIdAsync(CancellationToken ct = default)
        {
            return DbSet
                .AsNoTracking()
                .Where(p => p.FechaInicio <= DateTime.UtcNow && p.FechaFin >= DateTime.UtcNow)
                .Select(p => p.Id)
                .FirstOrDefaultAsync(ct);
        }

    }
}

                

        
    
