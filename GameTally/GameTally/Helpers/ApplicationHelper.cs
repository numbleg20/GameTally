using GameTally.Resources;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GameTally.Helpers
{
    public class ApplicationHelper
    {
        public static void HideFromAltTab(IntPtr windowHandle)
        {
            User32.SetWindowLong(windowHandle, User32.GWL_EXSTYLE,
                User32.GetWindowLong(windowHandle, User32.GWL_EXSTYLE) |
                User32.WS_EX_TOOLWINDOW);
        }

        /// <summary>
        /// Startup logic
        /// </summary>
        public static void Logic(bool value)
        {
            if (value) AddToStartup();
            else RemoveFromStartup();
        }

        static void AddToStartup()
        {
            string appPath = Application.ExecutablePath;

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue(Config.APPLICATION_NAME, appPath);
            registryKey.Close();
        }

        static void RemoveFromStartup()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (registryKey != null)
            {
                registryKey.DeleteValue(Config.APPLICATION_NAME, false);
                registryKey.Close();
            }
        }

        public static bool AddedToStartup()
        {
            return (Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                true).GetValueNames().Contains(Config.APPLICATION_NAME));
        }
    }
}