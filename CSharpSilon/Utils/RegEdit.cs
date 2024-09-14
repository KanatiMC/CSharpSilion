using System;
using Microsoft.Win32;

namespace CSharpSilon.Utils
{

    public class Regedit
    {
        string regedit_path = null;

        public Regedit(string regedit_path)
        {
            this.regedit_path = regedit_path;
        }

        public string Read(string key)
        {
            using (RegistryKey b = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var k = b.OpenSubKey(regedit_path))
            {
                try
                {
                    if (k != null)
                        return k.GetValue(key) == null ? "" : k.GetValue(key).ToString();
                }
                catch (Exception ex)
                {
                }

                return "";
            }
        }

        public bool SetValue(string key, object value)
        {
            using (var a = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var k = a.CreateSubKey(regedit_path, true))
            {
                try
                {
                    k.SetValue(key, value);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }

        }
    }
}