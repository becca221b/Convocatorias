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
                            Titulo: e.TituloGrado,
                            AnioGraduacion: e.AnioGraduacion
                        ))
                        .ToList()
                        .AsReadOnly(),
                    c.ExperienciasDocente
                        .Select(ed => new ExperienciaDocenteResponse(
                            Materia: ed.Cargo, // Asumiendo que "Cargo" representa la materia impartida
                            Institucion: ed.Institucion,
                            FechaInicio: ed.DesdePeriodo,
                            FechaFin: ed.HastaPeriodo
                        ))
                        .ToList()
                        .AsReadOnly(),
                    c.ExperienciasInvExt
                        .Select(ei => new ExperienciaInvExtResponse(
                            Descripcion: ei.Descripcion,
                            TipoExperiencia: ei.Tipo.ToString(), // Convertir el enum a string para la respuesta
                            TieneExperiencia: ei.TieneExperiencia,
                            ParticipacionInvExt: ei.ParticipoComo.ToString() // Convertir el enum a string para la respuesta
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
