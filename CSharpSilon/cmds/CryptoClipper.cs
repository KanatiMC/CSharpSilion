using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class CryptoClipper : ModuleBase<SocketCommandContext>
    {
        [Command("start-clipper")]
        public async Task StartClipper()
        {
        }

        [Command("stop-clipper")]
        public async Task StopClipper()
        {
        }
    }
}