using System.Drawing;
using System.Windows.Forms;

namespace GameTally.Models
{
    public class MenuStripStyle : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(45, 45, 45); }
        }
        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(30, 30, 30); }
        }
        public override Color MenuBorder
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(45, 45, 45); }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(45, 45, 45); }
        }

        public override Color ToolStripBorder
        {
            get { return ColorTranslator.FromHtml("#4C4A48"); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(45, 45, 45); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(45, 45, 45); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return ColorTranslator.FromHtml("#404040"); }
        }

        public override Color ToolStripGradientBegin
        {
            get { return ColorTranslator.FromHtml("#404040"); }
        }

        public override Color ToolStripGradientEnd
        {
            get { return ColorTranslator.FromHtml("#404040"); }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return ColorTranslator.FromHtml("#404040"); }
        }
    }
}
