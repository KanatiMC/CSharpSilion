using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Bsod : ModuleBase<SocketCommandContext>
    {
        [Command("bsod")]
        public async Task BSoD()
        {
        }
    }
}