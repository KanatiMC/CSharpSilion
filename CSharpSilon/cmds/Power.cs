using System.Threading.Tasks;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class Power : ModuleBase<SocketCommandContext>
    {
        [Command("restartpc"), Alias("rpc"), Summary("Restarts The User's PC")]
        public async Task RestartPC()
        {
            await ReplyAsync("Restarting Pc...");
            Utils.App.execute("Shutdown /r /f /t 0");
        }

        [Command("shutdown"), Alias("sd"), Summary("Shuts Down The User's PC")]
        public async Task ShutdownPC()
        {
            await ReplyAsync("Shuting Down Pc...");
            Utils.App.execute("shutdown /s /f /t 0");
        }

        [Command("logoff"), Alias("lo"), Summary("Logs The User Out Of Their PC")]
        public async Task Logoff()
        {
            await ReplyAsync("Logging Out Of The PC...");
            Utils.App.execute("Shutdown /l /f");
        }
    }
}