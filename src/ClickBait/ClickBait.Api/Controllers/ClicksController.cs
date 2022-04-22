using Microsoft.AspNetCore.Mvc;
using MediatR;
using ClickBait.Application.Commands;

namespace ClickBait.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClicksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClicksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> Post([FromRoute] Guid postId)
        {
            await _mediator.Send(new RegisterClickCommand(postId));
            return Accepted();
        }
    }
}
