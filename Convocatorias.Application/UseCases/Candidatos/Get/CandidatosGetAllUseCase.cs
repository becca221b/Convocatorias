using Convocatorias.Application.Common;
using Convocatorias.Application.Interfaces.Repositories;
using MediatR;


namespace Convocatorias.Application.UseCases.Candidatos.Get
{
    public sealed class CandidatosGetAllUseCase : IRequestHandler<CandidatoGetAllRequest, PagedResult<CandidatoResponse>>
    {
        private readonly ICandidatoRepository _candidatoRepository;

        public CandidatosGetAllUseCase(ICandidatoRepository candidatoRepository)
        {
            _candidatoRepository = candidatoRepository;
        }
        
        
        public async Task<PagedResult<CandidatoResponse>> Handle(
            CandidatoGetAllRequest request,
            CancellationToken cancellationToken)
        {
            var (candidatos, totalCandidatos) = await _candidatoRepository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            var items = candidatos
                .Select(c => new CandidatoResponse(
                    c.Id,
                    c.Nombre,
                    c.Apellido,
                    c.Email,
                    c.TieneDocumentacionRequerida(),
                    c.Educaciones
                        .Select(e => new EducacionResponse(
                            // Mapear propiedades de Educacion a EducacionResponse aquí
                        ))
                        .ToList()
                        .AsReadOnly(),
                    c.ExperienciasDocente
                        .Select(ed => new ExperienciaDocenteResponse(
                            // Mapear propiedades de ExperienciaDocente a ExperienciaDocenteResponse aquí
                        ))
                        .ToList()
                        .AsReadOnly(),
                    c.ExperienciasInvExt
                        .Select(ei => new ExperienciaInvExtResponse(
                            // Mapear propiedades de ExperienciaInvExt a ExperienciaInvExtResponse aquí
                        ))
                        .ToList()
                        .AsReadOnly()
                ))
                .ToList()
                .AsReadOnly();

            return new PagedResult<CandidatoResponse>(
                request.Page,
                request.PageSize,
                totalCandidatos,
                items
            );
        }
    }
}
