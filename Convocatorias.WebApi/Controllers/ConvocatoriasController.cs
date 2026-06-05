using Microsoft.AspNetCore.Mvc;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;
using System.Threading.Tasks;
using Convocatorias.Application.UseCases.Convocatorias.Create;
using Convocatorias.Application.UseCases.Convocatorias.Get;
using Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Convocatorias.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ConvocatoriasController : ControllerBase
    {
        private readonly ConvGetAllUseCase _getAllUseCase;
        private readonly ConvCreateUseCase _createUseCase;
        private readonly ConvGetByIdUseCase _getByIdUseCase;
        private readonly AsignarPeriodo _asignarPeriodo;

        public ConvocatoriasController(ConvGetAllUseCase getAllUseCase, ConvCreateUseCase createUseCase, ConvGetByIdUseCase getByIdUseCase, AsignarPeriodo asignarPeriodo)
        {
            _getAllUseCase = getAllUseCase;
            _createUseCase = createUseCase;
            _getByIdUseCase = getByIdUseCase;
            _asignarPeriodo = asignarPeriodo;
        }
        // GET: api/convocatorias
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var convocatorias = await _getAllUseCase.ExecuteAsync(ct);
            return Ok(convocatorias);
        }

        // GET api/<ConvocatoriaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var convocatoria = await _getByIdUseCase.ExecuteAsync(Guid.Parse(id.ToString()));
            return Ok(convocatoria);
        }

        // POST api/<ConvocatoriaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConvCreateRequest request, CancellationToken ct)
        {
            var response = await _createUseCase.ExecuteAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPatch("periodo")]
        public async Task<IActionResult> AsignarPeriodo([FromBody] AsignarPeriodoRequest request, CancellationToken ct)
        {
            var response = await _asignarPeriodo.Asignar(request);
            return Ok(response);
        }

    }
}
