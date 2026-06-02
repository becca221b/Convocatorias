using Convocatorias.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Periodos.Get
{
    public sealed class PeriodoGetVigenteId
    {
        private readonly IPeriodoRepository _repository;
        public PeriodoGetVigenteId(IPeriodoRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid?> ExecuteAsync(CancellationToken ct = default)
        {
            var periodoId = await _repository.GetVigenteIdAsync(ct);
            return periodoId;
        }
    }
}
