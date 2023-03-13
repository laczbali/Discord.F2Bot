using Discord.Core;
using Microsoft.AspNetCore.Mvc;

namespace Discord.F2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractionController : ControllerBase
    {
        private readonly DiscordRequestHandler requestHandler;

        public InteractionController(DiscordRequestHandler requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        [HttpPost("handler")]
        public async Task<IActionResult> HandlerAsync() => await this.requestHandler.ImmediateHandlerAsync(Request);
    }
}