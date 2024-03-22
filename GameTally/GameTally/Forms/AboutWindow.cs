using GameTally.Helpers;
using GameTally.Resources;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameTally.Forms
{
    public partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();

            label3.Text += Config.AUTHOR;

            /// <summary>
            /// Set Mica title bar if windows version is 11
            /// </summary>
            if (WindowsVersionHelper.IsWindows11())
            {
                var preference = Convert.ToInt32(true);
                dwmapi.DwmSetWindowAttribute(this.Handle,
                                      dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                                      ref preference, sizeof(uint));
            }

            linkLabel1.LinkClicked += (sender, e) => Process.Start(Config.STEAM_GRID_DB_LINK);
        }
    }
}
