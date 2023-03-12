using Discord.Core;
using Microsoft.AspNetCore.Mvc;

namespace Discord.F2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractionController : ControllerBase
    {
        private readonly RequestHandler requestHandler;

        public InteractionController(RequestHandler requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        [HttpPost]
        public async Task<IActionResult> HandlerAsync() => await this.requestHandler.HandlerAsync(Request);
    }
}