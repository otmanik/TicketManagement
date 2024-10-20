using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Tickets.Commands;
using TicketManagementSystem.Application.Features.Tickets.Queries;

namespace TicketManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateTicket([FromBody] CreateTicketCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<TicketListVm>> GetTickets([FromQuery] GetTicketListQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var command = new DeleteTicketCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}