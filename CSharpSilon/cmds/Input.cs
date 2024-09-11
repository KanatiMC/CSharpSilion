using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Input : ModuleBase<SocketCommandContext>
    {
        private static bool inputBlocked = false;
        
        [Command("block-input")]
        public async Task BlockInput()
        {
        }

        [Command("unblock-input")]
        public async Task UnblockInput()
        {
        }
    }
}