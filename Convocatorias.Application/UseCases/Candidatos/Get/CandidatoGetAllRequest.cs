using MediatR;


namespace Convocatorias.Application.UseCases.Candidatos.Get
{
    public sealed record CandidatoGetAllRequest() : IRequest<IReadOnlyCollection<CandidatoResponse>>;
}
