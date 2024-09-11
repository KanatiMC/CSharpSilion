using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace CSharpSilon.cmds
{
    public class FileExplorer : ModuleBase<SocketCommandContext>
    {
        private static ConcurrentDictionary<ulong, string> _userDirectories = new ConcurrentDictionary<ulong, string>();

        [Command("explore")]
        [Summary("Allows navigation through files and directories.")]
        public async Task ExploreDirectoryAsync(string directory = "")
        {
            if (!_userDirectories.TryGetValue(Context.User.Id, out var currentDirectory))
            {
                // Initialize to the root of the current drive
                currentDirectory = Path.GetPathRoot(Directory.GetCurrentDirectory());
                _userDirectories[Context.User.Id] = currentDirectory;
            }

            if (!string.IsNullOrEmpty(directory))
            {
                var newDirectory = Path.Combine(currentDirectory, directory);

                if (Directory.Exists(newDirectory))
                {
                    currentDirectory = newDirectory;
                    _userDirectories[Context.User.Id] = currentDirectory;
                }
                else if (directory == "..")
                {
                    var parentDirectory = Directory.GetParent(currentDirectory);
                    if (parentDirectory != null)
                    {
                        currentDirectory = parentDirectory.FullName;
                        _userDirectories[Context.User.Id] = currentDirectory;
                    }
                }
                else if (directory.Length >= 2 && directory[1] == ':' && directory[2] == '\\')
                {
                    // Handle absolute paths to different drives (e.g., D:\)
                    if (Directory.Exists(directory))
                    {
                        currentDirectory = directory;
                        _userDirectories[Context.User.Id] = currentDirectory;
                    }
                    else
                    {
                        await ReplyAsync($"Drive or directory \"{directory}\" does not exist.");
                        return;
                    }
                }
                else
                {
                    await ReplyAsync($"Directory \"{directory}\" does not exist.");
                    return;
                }
            }

            var directories = Directory.GetDirectories(currentDirectory);
            var files = Directory.GetFiles(currentDirectory);

            var response = $"**Current Directory: \"{currentDirectory}\"**\n";
            response += "Use 'explore <directory>' to navigate, '..' to go up a level\n\n";

            response += "**Directories:**\n";
            response += directories.Length > 0
                ? string.Join("\n", Array.ConvertAll(directories, dir => $"> {Path.GetFileName(dir)}"))
                : "None";

            response += "\n\n**Files:**\n";
            response += files.Length > 0
                ? string.Join("\n", Array.ConvertAll(files, file => $"> {Path.GetFileName(file)}"))
                : "None";

            await SendMessageInChunksAsync(response);
        }


        [Command("reset")]
        [Summary("Resets the current directory to the root directory.")]
        public async Task ResetDirectoryAsync()
        {
            var rootDirectory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            _userDirectories[Context.User.Id] = rootDirectory;

            await ReplyAsync($"Directory reset to root: `{rootDirectory}`.");
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

    }
}