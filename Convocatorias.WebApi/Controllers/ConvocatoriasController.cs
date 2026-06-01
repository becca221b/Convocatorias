using Microsoft.AspNetCore.Mvc;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;
using System.Threading.Tasks;
using Convocatorias.Application.UseCases.Convocatorias.Create;
using Convocatorias.Application.UseCases.Convocatorias.Get;

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

        public ConvocatoriasController(ConvGetAllUseCase getAllUseCase, ConvCreateUseCase createUseCase, ConvGetByIdUseCase getByIdUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _createUseCase = createUseCase;
            _getByIdUseCase = getByIdUseCase;
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
        public async Task<string> GetById(int id)
        {
            var convocatoria = await _getByIdUseCase.ExecuteAsync(Guid.Parse(id.ToString())));
            return Ok(convocatoria);
        }

        // POST api/<ConvocatoriaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConvCreateRequest request, CancellationToken ct)
        {
            var response = await _createUseCase.ExecuteAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT api/<ConvocatoriaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ConvocatoriaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
