using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class ScreenManipulation : ModuleBase<SocketCommandContext>
    {
        [Command("display-graphic")]
        public async Task DisplayGraphic()
        {
        }

        [Command("display-glitch")]
        public async Task DisplayGlitch()
        {
        }

        [Command("graphic-file")]
        public async Task GraphicFile()
        {
        }
    }
}