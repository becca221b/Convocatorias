using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.UseCases.Postulaciones.Get
{
    public sealed class GetPostulacionesByConvocatoriaId
    {
        private readonly IPostulacionRepository _repository;

        public GetPostulacionesByConvocatoriaId(IPostulacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Postulacion>> ExecuteAsync(Guid convocatoriaId, CancellationToken ct)
        {
            var postulaciones = await _repository.GetPostulacionesByConvocatoriaIdAsync(convocatoriaId, ct);
            return postulaciones.ToList();
        }
    }
}
