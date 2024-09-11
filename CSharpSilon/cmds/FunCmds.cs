using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CSharpSilon.cmds
{
    public class FunCmds : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Returns pong")]
        public async Task Ping()
        {
            await ReplyAsync("Ping Pongy").ConfigureAwait(false);
        }
    }
}