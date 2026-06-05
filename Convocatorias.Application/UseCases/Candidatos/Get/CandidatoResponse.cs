
namespace Convocatorias.Application.UseCases.Candidatos.Get
{
    public sealed record CandidatoResponse
    (
        Guid Id,
        string Nombre,
        string Apellido,
        
        string Email,
        bool TieneDocumentacionRequerida,
        IReadOnlyCollection<EducacionResponse> Educaciones,
        IReadOnlyCollection<ExperienciaDocenteResponse> ExperienciaDocente,
        IReadOnlyCollection<ExperienciaInvExtResponse> ExperienciasInvExt
    );

        public sealed record EducacionResponse
        (
            string Institucion,
            string Titulo,
            int AnioGraduacion
        );

        public sealed record ExperienciaDocenteResponse
        (
            string Materia,
            string Institucion,
            DateTime FechaInicio,
            DateTime? FechaFin
        );

        public sealed record ExperienciaInvExtResponse
        (
            string Descripcion,
            string Institucion,
            DateTime FechaInicio,
            DateTime? FechaFin
        );
}
