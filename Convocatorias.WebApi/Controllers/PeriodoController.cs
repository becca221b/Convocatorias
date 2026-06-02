using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Periodos.Create;
using Convocatorias.Application.UseCases.Periodos.Get;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Convocatorias.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodoController : ControllerBase
    {
        private readonly PeriodoCreateUseCase _createUseCase;
        private readonly PeriodoGetAllUseCase _getAllUseCase;
        private readonly PeriodoGetByIdUseCase _periodoGetById;
        private readonly PeriodoGetVigenteId _periodoGetVigenteId;
        private readonly PeriodoGetVigenteUseCase _periodoGetVigente;

        public PeriodoController(PeriodoGetAllUseCase getAllUseCase, PeriodoCreateUseCase createUseCase, PeriodoGetByIdUseCase periodoGetById, PeriodoGetVigenteId periodoGetVigenteId, PeriodoGetVigenteUseCase periodoGetVigente)
        {
            _createUseCase = createUseCase;
            _getAllUseCase = getAllUseCase;
            _periodoGetById = periodoGetById;
            _periodoGetVigenteId = periodoGetVigenteId;
            _periodoGetVigente = periodoGetVigente;

        }

        // GET: api/<PeriodoController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _getAllUseCase.ExecuteAsync();
            return Ok(response);
        }

        // GET api/<PeriodoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _periodoGetById.ExecuteAsync(id);
            if (response is null) return NotFound();
            return Ok(response);
        }

        //GET api/<PeriodoController>/vigente
        [HttpGet("vigente")]
        public async Task<IActionResult> GetVigente()
        {
            var response = await _periodoGetVigente.ExecuteAsync();
            if (response is null) return NotFound();
            return Ok(response);
        }

        //GET api/<PeriodoController>/vigenteId
        [HttpGet("vigenteId")]
        public async Task<IActionResult> GetVigenteId()
        {
            var response = await _periodoGetVigenteId.ExecuteAsync();
            if (response is null) return NotFound();
            return Ok(response);
        }

        // POST api/<PeriodoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PeriodoCreateRequest request)
        {
            
            var response = await _createUseCase.HandleAsync(request);
            return Ok(response);
        }

        // PUT api/<PeriodoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PeriodoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
