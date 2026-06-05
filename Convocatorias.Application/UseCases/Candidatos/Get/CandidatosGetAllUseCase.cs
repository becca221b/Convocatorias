using Convocatorias.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Candidatos.Get
{
    public sealed class CandidatosGetAllUseCase : IRequestHandler<CandidatoGetAllRequest, IReadOnlyCollection<CandidatoResponse>>
    {
        private readonly ICandidatoRepository _candidatoRepository;

        public CandidatosGetAllUseCase(ICandidatoRepository candidatoRepository)
        {
            _candidatoRepository = candidatoRepository;
        }
        
        
        public Task<IReadOnlyCollection<CandidatoResponse>> Handle(CandidatoGetAllRequest request, CancellationToken cancellationToken)
        {
            var candidatos = _candidatoRepository.GetAllAsync(cancellationToken);

            return candidatos
                .Select(
                    c => new CandidatoResponse(
                        c.Id,
                        c.Nombre,
                        c.Apellido,
                        c.Email,
                        c.TieneDocumentacionRequerida(),
                        c.Educaciones.Select(e => new CandidatoResponse.EducacionResponse(
                            e.Institucion,
                            e.Titulo,
                            e.AnioGraduacion
                        )).ToList(),
                        c.ExperienciaDocente.Select(ed => new CandidatoResponse.ExperienciaDocenteResponse(
                            ed.Materia,
                            ed.Institucion,
                            ed.FechaInicio,
                            ed.FechaFin
                        )).ToList(),
                        c.ExperienciasInvExt.Select(ei => new CandidatoResponse.ExperienciaInvExtResponse(
                            ei.Descripcion,
                            ei.Institucion,
                            ei.FechaInicio,
                            ei.FechaFin
                        )).ToList()
                    )
                )
                .ToList()
                .AsReadOnly();
        }
    }
}
