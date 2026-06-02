using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Periodos.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Periodos.Get
{
    public sealed class PeriodoGetByIdUseCase
    {
        private readonly IPeriodoRepository _repository;
        public PeriodoGetByIdUseCase(IPeriodoRepository repository)
        {
            _repository = repository;
        }
        public async Task<PeriodoResponse?> ExecuteAsync(Guid id, CancellationToken ct = default)
        {
            var periodo = await _repository.GetByIdAsync(id, ct);
            if (periodo is null) return null;

            return new PeriodoResponse(
                periodo.Id,
                periodo.Orden,
                periodo.Cuatrimestre.ToString(),
                periodo.Anio,
                periodo.FechaInicio,
                periodo.FechaFin,
                periodo.EstaVigente(DateTime.UtcNow)
            );
        }
}
