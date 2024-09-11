using Discord;
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
            string cmds = "Commands For CsSilon\n```";
            foreach (var module in _commands.Modules)
            {
                foreach (var cmd in module.Commands)
                {
                    if (!string.IsNullOrEmpty(cmd.Summary))
                    {
                        cmds += $"{cmd.Aliases.FirstOrDefault()}: {cmd.Summary ?? "No description provided."}\n";
                    }
                }
            }

            cmds += "```";

            await ReplyAsync(cmds);
        }
    }
}