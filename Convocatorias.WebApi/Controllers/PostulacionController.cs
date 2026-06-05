using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Postulaciones.Get;
using Convocatorias.Application.UseCases.Postularse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Convocatorias.WebApi.Controllers
{
    public class PostulacionController : Controller
    {
        private readonly GetAll _getAllUseCase;
        private readonly GetById _getByIdUse;
        private readonly GetPostulacionesByConvocatoriaId _getByConvocatoriaIdUseCase;
        private readonly PostularseUseCase _createUseCase;

        public PostulacionController(GetAll getAllUseCase, GetById getByIdUse, GetPostulacionesByConvocatoriaId getByConvocatoriaIdUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUse = getByIdUse;
            _getByConvocatoriaIdUseCase = getByConvocatoriaIdUseCase;
            
        }

        [HttpGet]
        // GET: PostulacionController
        public async Task<ActionResult> GetAll(CancellationToken ct)
        {
            var postulaciones = await _getAllUseCase.ExecuteAsync(ct);
            return View(postulaciones);
        }

        // GET: PostulacionController/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var postulacion = await _getByIdUse.ExecuteAsync(id, CancellationToken.None);
            if (postulacion == null)
            {
                return NotFound();
            }
            return Ok(postulacion);
        }

        

        // POST: api/PostulacionController
        [HttpPost]
        public async Task<ActionResult> Postularse([FromBody] PostularseRequest request, CancellationToken ct)
        {
            var createdPostulacion = await _createUseCase.Postular(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = createdPostulacion.PostulacionId }, createdPostulacion);
        }

       

        
    }
}
