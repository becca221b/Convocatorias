using Convocatorias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.Interfaces.Repositories
{
    public interface IConvPeriodoRepository
    {
        Task AddAsync(ConvocatoriaPeriodo convocatoriaPeriodo);
         Task<IEnumerable<ConvocatoriaPeriodo>> GetByConvocatoriaIdAsync(Guid convocatoriaId);
         Task UpdateAsync(ConvocatoriaPeriodo convocatoriaPeriodo);
         Task<Guid> ObtenerPeriodoVigente();
    }
}
