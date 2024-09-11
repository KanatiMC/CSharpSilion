using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class WebsiteBlocker : ModuleBase<SocketCommandContext>
    {
        [Command("block-website")]
        public async Task BlockWebsite()
        {
        }

        [Command("unblock-website")]
        public async Task UnblockWebsite()
        {
        }
    }
}