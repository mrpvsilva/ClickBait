using Microsoft.AspNetCore.Mvc;
using MediatR;
using ClickBait.Application.Commands;
using ClickBait.Application.Services;

namespace ClickBait.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClicksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IClickCountService _clickCountService;

        public ClicksController(IMediator mediator, IClickCountService clickCountService)
        {
            _mediator = mediator;
            _clickCountService = clickCountService;
        }

        [HttpGet]
        public async Task<IActionResult> Post([FromQuery] Guid? postId)
        {
            var clicksCounts = await _clickCountService.Get(x => !postId.HasValue || x.PostId == postId);
            return Ok(clicksCounts);
        }

        [HttpPost("{postId}")]
        public IActionResult Post([FromRoute] Guid postId)
        {
            _ = _mediator.Send(new RegisterClickCommand(postId));
            return Accepted();
        }
    }
}
