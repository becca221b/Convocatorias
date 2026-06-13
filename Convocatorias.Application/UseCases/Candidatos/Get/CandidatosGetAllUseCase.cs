using Convocatorias.Application.Common;
using Convocatorias.Application.Interfaces.Repositories;
using MediatR;
using System.Linq;


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
                            e.Id,
                            e.TituloGrado,
                            e.AnioGraduacion,
                            e.PosgradoStatus.ToString(), // <-- Conversión explícita a string
                            e.PosgradoNombre,
                            e.TipoFormacion,
                            e.Documentos
                                .Select(d => new DocumentoResponse(
                                    d.Id,
                                    d.TipoDocumento,
                                    d.Url
                                ))
                                .ToList()
                                .AsReadOnly()
                        ))
                        .ToList()
                        .AsReadOnly(),
                        c.ExperienciasDocente
                        .Select(ed => new ExperienciaDocenteResponse(
                            ed.Id,
                            ed.AniosExperiencia,
                            ed.Nivel.ToString(),
                            ed.Institucion,
                            ed.Cargo,
                            ed.DesdePeriodo,
                            ed.HastaPeriodo,
                            ed.Documentos
                                .Select(d => new DocumentoResponse(
                                    d.Id,
                                    d.TipoDocumento,
                                    d.Url
                                ))
                                .ToList()
                                .AsReadOnly()
                        ))
                        .ToList()
                        .AsReadOnly(),
                        c.ExperienciasInvExt
                        .Select(ei => new ExperienciaInvExtResponse(
                            ei.Id,
                            ei.Tipo.ToString(),
                            ei.TieneExperiencia,
                            ei.ParticipoComo.ToString(),
                            ei.Descripcion,
                            ei.Documentos
                                .Select(d => new DocumentoResponse(
                                    d.Id,
                                    d.TipoDocumento,
                                    d.Url
                                ))
                                .ToList()
                                .AsReadOnly()
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
