using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;


namespace Convocatorias.Application.UseCases.Postularse
{
    public class PostularseUseCase
    {
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IConvPeriodoRepository _convocatoriaPeriodoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostularseUseCase(IPostulacionRepository postulacionRepository, IConvocatoriaRepository convocatoriaRepository, IConvPeriodoRepository convocatoriaPeriodoRepository, IUnitOfWork unitOfWork)
        {
            _postulacionRepository = postulacionRepository;
            _convocatoriaRepository = convocatoriaRepository;
            _convocatoriaPeriodoRepository = convocatoriaPeriodoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostularseResponse> Postular(PostularseRequest request)
        {
                     

            //Obtener convocatoria
            var convocatoria = await _convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId);

            if (convocatoria == null)
                throw new ArgumentException("Convocatoria no encontrada");
            
            //Verificar que la convocatoria esté abierta
            if (!convocatoria.ValidarAbierta())
                throw new InvalidOperationException("La convocatoria está cerrada");


            //Verificar si es el periodo de la convocatoria es el vigente
            var periodoId = convocatoria.ObtenerPeriodoActual();
            var convocatoriaPeriodoActualId = await _convocatoriaPeriodoRepository.ObtenerPeriodoVigente();

            if (periodoId != convocatoriaPeriodoActualId)
            {
                throw new InvalidOperationException("La convocatoria no está en periodo vigente");
            }

            //Verificar que el candidato no se haya postulado previamente a esta convocatoria
            var postulacionExistente = await _postulacionRepository.PostulacionExistsAsync(request.ConvocatoriaId, request.CandidatoId);
            if (postulacionExistente != null)
                throw new InvalidOperationException("El candidato ya se ha postulado a esta convocatoria");

            //Crear la postulación
            var postulacion = new Postulacion(request.ConvocatoriaId, request.CandidatoId);

            //Guardar la postulación
            await _postulacionRepository.AddAsync(postulacion);
            await _unitOfWork.SaveChanges();
            //Respuesta
            return new PostularseResponse
            {
                PostulacionId = postulacion.Id,
                Mensaje = "Postulación realizada con éxito"
            };
        }
    }
}
