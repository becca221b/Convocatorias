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

        public async Task Execute(PostularseRequest request)
        {
         
        }
    }
}
