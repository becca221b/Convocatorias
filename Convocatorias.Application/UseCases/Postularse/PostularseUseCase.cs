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
      
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeriodoRepository _periodoRepository;

        public PostularseUseCase(IPostulacionRepository postulacionRepository, IConvocatoriaRepository convocatoriaRepository, ICandidatoRepository candidatoRepository, IPeriodoRepository periodoRepository, IUnitOfWork unitOfWork)
        {
            _postulacionRepository = postulacionRepository;
            _convocatoriaRepository = convocatoriaRepository;
            _periodoRepository = periodoRepository;
            _candidatoRepository = candidatoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostularseResponse> Postular(PostularseRequest request)
        {

            //Obtener convocatoria
            var convocatoria = await _convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId);

            if (convocatoria == null)
                throw new ArgumentException("Convocatoria no encontrada");
            
            //Verificar que la convocatoria esté abierta
            if (!convocatoria.estaAbierta())
                throw new InvalidOperationException("La convocatoria está cerrada");


            //Verificar si es el periodo de la convocatoria es el vigente
           var periodoVigente = await _periodoRepository.GetVigenteAsync(DateTime.UtcNow);

            if(periodoVigente == null || periodoVigente.Id != convocatoria.ObtenerPeriodoActual())
                throw new InvalidOperationException("No se pueden realizar postulaciones fuera del periodo vigente de la convocatoria");


            //Verificar que el candidato no se haya postulado previamente a esta convocatoria
            var postulacionExistente = await _postulacionRepository.PostulacionExistsAsync(request.ConvocatoriaId, request.CandidatoId);
            if (postulacionExistente)
                throw new InvalidOperationException("El candidato ya se ha postulado a esta convocatoria");

            //Verificar que el candidato cuente con la documentación requerida para postularse a esta convocatoria
            var candidato = await _candidatoRepository.GetByIdAsync(request.CandidatoId);
            var cumpleRequisitos = candidato.TieneDocumentacionRequerida();
            if (!cumpleRequisitos)  
                throw new InvalidOperationException("El candidato no cumple con los requisitos para postularse a esta convocatoria");

            //Crear la postulación
            var postulacion = new Postulacion(request.ConvocatoriaId, request.CandidatoId);

            //Guardar la postulación
            await _postulacionRepository.AddAsync(postulacion);
            await _unitOfWork.SaveChangesAsync();
            //Respuesta
            return new PostularseResponse
            {
                PostulacionId = postulacion.Id,
                Mensaje = "Postulación realizada con éxito"
            };
        }
    }
}
