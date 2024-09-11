using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Webcam : ModuleBase<SocketCommandContext>
    {
        [Command("webcam")]
        public async Task WebcamPic()
        {
        }

        [Command("photo")]
        public async Task Photo()
        {
        }
    }
}