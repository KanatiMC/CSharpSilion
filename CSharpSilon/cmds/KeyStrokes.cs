using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class KeyStrokes : ModuleBase<SocketCommandContext>
    {
        [Command("key")]
        public async Task Key()
        {
        }
    }
}