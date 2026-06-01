using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Convocatorias.Get;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;


namespace Convocatorias.Application.UseCases.Convocatorias.Get
{
    public sealed class GetAbiertasUseCase
    {
        private readonly IConvocatoriaRepository _repository;
        public GetAbiertasUseCase(IConvocatoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<ConvocatoriaResponse>> ExecuteAsync(CancellationToken ct = default)
        {
            var convocatorias = await _repository.GetAbiertasAsync(ct);
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
