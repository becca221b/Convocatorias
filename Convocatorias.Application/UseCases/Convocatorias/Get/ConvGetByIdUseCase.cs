using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Convocatorias.Get
{
    public sealed class ConvGetByIdUseCase
    {
        private readonly IConvocatoriaRepository _repository;
        public ConvGetByIdUseCase(IConvocatoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<ConvocatoriaResponse> ExecuteAsync(Guid id, CancellationToken ct = default)
            {
                // Aquí iría la lógica para obtener la convocatoria por su ID desde el repositorio
                var convocatoria = await _repository.GetByIdAsync(id, ct);

            return new ConvocatoriaResponse
            (
                convocatoria.Id,
                convocatoria.Asignatura,
                convocatoria.SedeId,
                convocatoria.FacultadId,
                convocatoria.CarreraId,
                convocatoria.Modalidad.ToString(),
                convocatoria.Status.ToString()
            );
                   
        }
}
