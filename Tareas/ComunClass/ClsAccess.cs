using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Tareas.ComunClass
{
    class ClsRegedit
    {
        private const String PATH_REGISTRY=@"SOFTWARE\Control Tareas";

        static public string GetSetting(string section, string key, string sDefault)
        {
            try
            {
                // Guardamos los datos del usuario y de la aplicación
                // HKEY_CURRENT_USER\SOFTWARE\Control Tareas
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(PATH_REGISTRY + "\\" + section);
                string s = sDefault;
                if (rk != null)
                    s = (string)rk.GetValue(key);
                //
                return s;
            }
            catch
            {
                return "";
            }

        }

        static public string GetSetting(string section, string key)
        {
            return GetSetting( section, key, "");
        }

        static public void SaveSetting(string section, string key, string setting)
        {
            try
            {
                // Guardamos los datos del usuario y de la aplicación
                // HKEY_CURRENT_USER\SOFTWARE\Control Tareas
                RegistryKey rk = Registry.CurrentUser.CreateSubKey(PATH_REGISTRY + "\\" + section);
                rk.SetValue(key, setting);
            }
            catch {
                
            }
        }



    }
}
