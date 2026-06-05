using Convocatorias.Application.UseCases.Candidatos.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Convocatorias.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatoController : ControllerBase
    {
        private readonly ISender _sender;

        public CandidatoController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        
        // GET: api/<CandidatoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CandidatoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CandidatoController>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CandidatoRequest request)
        {
            var result = await _sender.Send(request);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        // PUT api/<CandidatoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CandidatoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
