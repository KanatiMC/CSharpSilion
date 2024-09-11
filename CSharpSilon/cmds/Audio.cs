using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Audio : ModuleBase<SocketCommandContext>
    {
        [Command("volume")]
        public async Task Volume()
        {
        }

        [Command("play")]
        public async Task Play()
        {
        }
    }
}