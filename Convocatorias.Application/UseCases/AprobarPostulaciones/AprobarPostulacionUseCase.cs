using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Application.UseCases.AprobarPostulaciones
{
    public class AprobarPostulacionUseCase
    {
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AprobarPostulacionUseCase(IPostulacionRepository postulacionRepository, IConvocatoriaRepository convocatoriaRepository, IUnitOfWork unitOfWork)
        {
            _postulacionRepository = postulacionRepository;
            _convocatoriaRepository = convocatoriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AprobarPostulacionResponse> Aprobar(AprobarPostulacionRequest request)
        {
            //Obtener postulación
            var postulacion = await _postulacionRepository.GetByIdAsync(request.PostulacionId);
            if (postulacion == null)
                throw new ArgumentException("Postulación no encontrada");

            //Obtener convocatoria
            var convocatoria = await _convocatoriaRepository.GetByIdAsync(postulacion.ConvocatoriaId);
            if (convocatoria == null)
                throw new ArgumentException("Convocatoria no encontrada");

            // Ver si la convocatoria está abierta
            if (!convocatoria.ValidarAbierta())
                throw new InvalidOperationException("La convocatoria ya está cerrada, tiene una postulación ya aprobada");

            //Aprobar la postulación
            postulacion.CambiarEstado(EstadoPostulacion.Aprobada);
            convocatoria.CerrarConvocatoria();
            //Actualizar la postulación
            await _postulacionRepository.UpdateAsync(postulacion);
            await _unitOfWork.SaveChangesAsync();
            //Respuesta
            return new AprobarPostulacionResponse
            {
                PostulacionId = postulacion.Id,
                CandidatoName = "", // Aquí podríamos obtener el nombre del candidato a través de una consulta adicional
                Estado = postulacion.Estado.ToString(),
                ConvocatoriaName = convocatoria.Asignatura,
            };
        }
    }
}
