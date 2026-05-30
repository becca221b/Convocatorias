using Convocatorias.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Convocatorias.GetAll
{
    public sealed class ConvGetAllUseCase
    {
        private readonly IConvocatoriaRepository _repository;
        public ConvGetAllUseCase(IConvocatoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IReadOnlyCollection<ConvocatoriaResponse>> ExecuteAsync(CancellationToken ct = default)
        {
            var convocatorias = await _repository.GetAllAsync(ct);
            return convocatorias.Select(c => new ConvocatoriaResponse(
                c.Id,
                c.Asignatura,
                c.SedeId,
                c.FacultadId,
                c.CarreraId,
                c.Modalidad.ToString(),
                c.Status.ToString()
            )).ToList();
        }
    }
}
