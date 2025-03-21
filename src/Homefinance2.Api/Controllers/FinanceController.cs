using HomeFinance2.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Homefinance2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FinanceController : ControllerBase
    {
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

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

        public async Task<IActionResult> Create(Finance finance)
        {
            return CreatedAtAction(nameof(Create), finance);
        }

        public async Task<IActionResult> Put(Guid id, Finance finance)
        {
            return NoContent();
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            return NoContent();
        }
    }
}
