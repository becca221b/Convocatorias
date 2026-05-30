using Microsoft.AspNetCore.Mvc;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Convocatorias.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ConvocatoriasController : ControllerBase
    {
        private readonly ConvGetAllUseCase _getAllUseCase;

        public ConvocatoriasController(ConvGetAllUseCase getAllUseCase)
        {
            _getAllUseCase = getAllUseCase;
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ConvocatoriaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
