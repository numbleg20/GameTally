using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTally.Forms
{
    public partial class MessageWindow : Form
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        public static bool ShowL(string title, string text)
        {
            bool s = false;
            var x = new MessageWindow();
            x.Text = title;
            x.label1.Text = text;

            x.button1.Click += (sender, e) =>
            {
                s = true;
                x.DialogResult = DialogResult.OK;
                x.Close();
            };
            x.button2.Click += (sender, e) =>
            {
                s = false;
                x.DialogResult = DialogResult.Cancel;
                x.Close();
            };
            x.ShowDialog();

            return s;
        }
    }
}
