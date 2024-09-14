using System;
using System.Collections.Generic;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;

        public HelpCommand(CommandService commands)
        {
            _commands = commands;
        }

        [Command("help"), Summary("Shows All Implemented Bot Commands")]
        public async Task HelpCommandAsync()
        {
            const int maxMessageLength = 2000;
            List<string> commandList = new List<string>();
    
            foreach (var module in _commands.Modules)
            {
                foreach (var cmd in module.Commands)
                {
                    if (!string.IsNullOrEmpty(cmd.Summary))
                    {
                        var firstAlias = cmd.Aliases.First();
                        var otherAliases = cmd.Aliases.Count > 1 ? $"({string.Join(", ", cmd.Aliases.Skip(1))})" : "";

                        var commandArgs = cmd.Parameters.Count > 0
                            ? $" [{string.Join(", ", cmd.Parameters.Select(p => $"{p.Name}: {p.Type.Name}{(p.IsOptional ? $" (Default: {p.DefaultValue})" : "")}"))}]"
                            : "";

                        var cmdEntry = $"> `{firstAlias}{(string.IsNullOrEmpty(otherAliases) ? "" : " " + otherAliases)}{commandArgs}`: {cmd.Summary}\n";
                        commandList.Add(cmdEntry);
                    }
                }
            }

            commandList.Sort();  // Sort commands alphabetically

            string cmds = "Commands For CsSilon\n";
            foreach (var cmdEntry in commandList)
            {
                if ((cmds.Length + cmdEntry.Length) > maxMessageLength)
                {
                    await ReplyAsync(cmds);
                    cmds = "";
                }

                cmds += cmdEntry;
            }

            if (!string.IsNullOrEmpty(cmds))
            {
                await ReplyAsync(cmds);
            }
        }


        
    }
}