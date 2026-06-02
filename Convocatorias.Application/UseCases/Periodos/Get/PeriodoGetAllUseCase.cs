using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Periodos.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Periodos.Get
{
    public sealed class PeriodoGetAllUseCase
    {
        private readonly IPeriodoRepository _repository;
        public PeriodoGetAllUseCase(IPeriodoRepository repository)
        {
            _repository = repository;
        }
        public async Task<IReadOnlyCollection<PeriodoResponse>> ExecuteAsync(CancellationToken ct = default)
        {

            var periodos = await _repository.GetAllAsync(ct);
            return periodos.Select(p => new PeriodoResponse(
                 p.Id,
                 p.Orden,
                 p.Cuatrimestre.ToString(),
                 p.Anio,
                 p.FechaInicio,
                 p.FechaFin,
                 p.EstaVigente(DateTime.UtcNow)
            )).ToList();


        }

    }
}
