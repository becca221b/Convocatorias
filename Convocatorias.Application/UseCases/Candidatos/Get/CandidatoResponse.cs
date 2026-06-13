
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
            Guid Id,
            string TituloGrado,
            int AnioGraduacion,
            string PosgradoStatus,
            string PosgradoNombre,
            string TipoFormacion,
            IReadOnlyCollection<DocumentoResponse> Documentos
        );

        public sealed record ExperienciaDocenteResponse
        (
            Guid Id,
            int AniosExperiencia,
            string Nivel,
            string Institucion,
            string Cargo,
            DateTime DesdePeriodo,
            DateTime HastaPeriodo,
            IReadOnlyCollection<DocumentoResponse> Documentos
        );

        public sealed record ExperienciaInvExtResponse
        (
            Guid Id,
            string Tipo,
            bool TieneExperiencia,
            string ParticipoComo,
            string Descripcion,
            IReadOnlyCollection<DocumentoResponse> Documentos
        );

        public sealed record DocumentoResponse(
            Guid Id,
            string TipoDocumento,
            string Url
        );
}
