using Convocatorias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface ICandidatoRepository
    {
            Task<Candidato?> GetByIdAsync(Guid id, CancellationToken ct = default);
            Task<List<Candidato>> GetAllAsync(CancellationToken ct = default);
            Task AddAsync(Candidato candidato, CancellationToken ct = default);
            
    }
}
