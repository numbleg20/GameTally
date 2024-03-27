using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTally.Helpers
{
    internal class ImageResolutionHelper
    {
        public static bool ImageSupported(int width, int height)
        {
            if (width == Config.SUPPORTED_RESOLUTIONS.SizeFirst.Width && height == Config.SUPPORTED_RESOLUTIONS.SizeFirst.Height)
            {
                return true;
            }
            else return false;
        }
    }
}
