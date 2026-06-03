using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Convocatorias.Create;
using Convocatorias.Application.UseCases.Periodos.Create;
using Convocatorias.Application.UseCases.Periodos.Get;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Convocatorias.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PeriodoController : ControllerBase
    {
        private readonly PeriodoCreateUseCase _createUseCase;
        private readonly PeriodoGetAllUseCase _getAllUseCase;
        private readonly PeriodoGetByIdUseCase _getByIdUseCase;
        private readonly PeriodoGetVigenteUseCase _getVigenteUseCase;

        public PeriodoController(PeriodoCreateUseCase createUseCase, PeriodoGetAllUseCase getAllUseCase, PeriodoGetByIdUseCase getByIdUseCase, PeriodoGetVigenteUseCase getVigenteUseCase)
        {
            _createUseCase = createUseCase;
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _getVigenteUseCase = getVigenteUseCase;
        }
        // GET: api/<PeriodoController>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var periodos = await _getAllUseCase.ExecuteAsync(ct);
            return Ok(periodos);

        }

        // GET api/<PeriodoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var periodo = await _getByIdUseCase.ExecuteAsync(id, ct);
            if (periodo == null)
            {
                return NotFound();
            }
            return Ok(periodo);
        }

        // GET api/<PeriodoController>/vigente
        [HttpGet("vigente")]
        public async Task<IActionResult> GetVigente(CancellationToken ct)
        {
            var periodo = await _getVigenteUseCase.ExecuteAsync(ct);
            if (periodo == null)
            {
                return NotFound();
            }
            return Ok(periodo);
        }

        //Post api/<PeriodoController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeriodoCreateRequest request, CancellationToken ct)
        {
            var periodoId = await _createUseCase.HandleAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = periodoId }, null);
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
