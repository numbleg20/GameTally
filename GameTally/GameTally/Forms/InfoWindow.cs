using GameTally.Helpers;
using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTally.Forms
{
    public partial class InfoWindow : Form
    {
        public InfoWindow()
        {
            InitializeComponent();

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
        }
    }
}
