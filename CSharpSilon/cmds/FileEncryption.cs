using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class FileEncryption : ModuleBase<SocketCommandContext>
    {
        [Command("encrypt")]
        public async Task Encrypt()
        {
        }

        [Command("decrypt")]
        public async Task Decrypt()
        {
        }
    }
}