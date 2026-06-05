using Convocatorias.Application.UseCases.AprobarPostulaciones;
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
        private readonly AprobarPostulacionUseCase _aprobarPostulacionUseCase;

        public PostulacionController(
            GetAll getAllUseCase,
            GetById getByIdUse,
            GetPostulacionesByConvocatoriaId getByConvocatoriaIdUseCase,
            PostularseUseCase postularse,
            AprobarPostulacionUseCase aprobarPostulacion
            )
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUse = getByIdUse;
            _getByConvocatoriaIdUseCase = getByConvocatoriaIdUseCase;
            _createUseCase = postularse;
            _aprobarPostulacionUseCase = aprobarPostulacion;
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

        //GET: PostulacionController/Convocatoria/5
        [HttpGet("Convocatoria/{convocatoriaId}")]
        public async Task<ActionResult> GetByConvocatoriaId(Guid convocatoriaId, CancellationToken ct)
        {
            var postulaciones = await _getByConvocatoriaIdUseCase.ExecuteAsync(convocatoriaId, ct);
            return Ok(postulaciones);
        }


        // POST: api/PostulacionController
        [HttpPost]
        public async Task<ActionResult> Postularse([FromBody] PostularseRequest request, CancellationToken ct)
        {
            var createdPostulacion = await _createUseCase.Postular(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = createdPostulacion.PostulacionId }, createdPostulacion);
        }

        //PATCH: api/PostulacionController/5
        [HttpPatch]
        public async Task<ActionResult> AprobarPostulacion([FromBody] AprobarPostulacionRequest request, CancellationToken ct)
        {
            var response = await _aprobarPostulacionUseCase.Aprobar(request, ct);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

    }
}
