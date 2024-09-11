using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Messenger : ModuleBase<SocketCommandContext>
    {
        [Command("msg")]
        public async Task MSG()
        {
        }
    }
}