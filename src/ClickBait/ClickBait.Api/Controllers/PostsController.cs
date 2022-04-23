using ClickBait.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClickBait.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _postService.Get(x => true);
            return Ok(all);
        }
    }
}
