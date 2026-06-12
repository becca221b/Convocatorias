using Convocatorias.Application.Common;
using MediatR;


namespace Convocatorias.Application.UseCases.Candidatos.Get
{
    public sealed record CandidatoGetAllRequest(
        int Page = 1,
        int PageSize = 20
    ) : IRequest<PagedResult<CandidatoResponse>>;
}
