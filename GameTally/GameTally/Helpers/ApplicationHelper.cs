using GameTally.Resources;
using Microsoft.Win32;
using System;
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
        static RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public static void AddToStartup()
        {
            try
            {
                registryKey.SetValue(Config.APPLICATION_NAME, Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        public static void DeleteFromStartup()
        {
            try
            {
                registryKey.DeleteValue(Config.APPLICATION_NAME, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        public static bool AddedToStartup()
        {
            return registryKey.GetValue(Config.APPLICATION_NAME) != null;
        }
    }
}