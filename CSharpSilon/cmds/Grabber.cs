using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Grabber : ModuleBase<SocketCommandContext>
    {
        [Command("grab")]
        public async Task Grab()
        {
        }
    }
}