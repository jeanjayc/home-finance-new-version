using System.Text.Json;
using HomeFinance2.Application.ApiService.CQRS.Commands.RegisterFinance;
using HomeFinance2.Application.FinanceService.DTO;
using HomeFinance2.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Homefinance2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FinanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FinanceController> _logger;

        public FinanceController(IMediator mediator, ILogger<FinanceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "O atributo Id é obrigatório para buscar um item",
                    Detail = $"Id vazio ou nulo"
                });
            }
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(FinanceDTO financeDto)
        {
            
            var financeToJson = JsonSerializer.Serialize(financeDto);
            var query = new RegisterFinanceCommand(financeToJson);
            await _mediator.Send(query);
            return CreatedAtAction(nameof(Create), financeDto);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, FinanceDTO finance)
        {
            return NoContent();
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return NoContent();
        }
    }
}
