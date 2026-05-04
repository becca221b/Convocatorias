using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;


namespace Convocatorias.Application.UseCases.Postularse
{
    public class PostularseUseCase
    {
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IConvocatoriaRepository _convocatoriaRepository;

        public PostularseUseCase(IPostulacionRepository postulacionRepository, IConvocatoriaRepository convocatoriaRepository)
        {
            _postulacionRepository = postulacionRepository;
            _convocatoriaRepository = convocatoriaRepository;
        }

        public async Task<PostularseResponse> Execute(PostularseRequest request)
        {
            if(request.ConvocatoriaId == Guid.Empty)
                throw new ArgumentException("Convocatoria inválida");

            if(request.CandidatoId == Guid.Empty)
                throw new ArgumentException("Candidato inválido");
            //Obtener convocatoria
            var convocatoria = await _convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId);
            if (convocatoria == null)
                throw new ArgumentException("Convocatoria no encontrada");
            
            //Verificar que la convocatoria esté disponible para postulación
            if (!await _convocatoriaRepository.EstaDisponibleAsync(request.ConvocatoriaId))
                throw new InvalidOperationException("La convocatoria no está disponible para postulación");


            //Verificar si es el periodo de postulación vigente
            

            //Crear la postulación
            var postulacion = new Postulacion(request.ConvocatoriaId, request.CandidatoId);

            //Guardar la postulación
            await _postulacionRepository.AddAsync(postulacion);

            //Respuesta
            return new PostularseResponse
            {
                PostulacionId = postulacion.Id,
                Mensaje = "Postulación realizada con éxito"
            };
        }
    }
}
