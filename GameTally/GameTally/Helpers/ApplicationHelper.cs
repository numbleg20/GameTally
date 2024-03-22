using System;

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
    }
}