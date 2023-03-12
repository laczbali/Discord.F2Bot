using Discord.Core;
using Discord.Core.Models;
using Discord.Core.Utils;

namespace Discord.F2.Providers
{
    public class InteractionProvider : InteractionsBase
    {
        public InteractionProvider(ClientConfig config) : base(config)
        {
        }

        public override ApplicationCommand[] GlobalCommands => new ApplicationCommand[]
        {

        };
    }
}
