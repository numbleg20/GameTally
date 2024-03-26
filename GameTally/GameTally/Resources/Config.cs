using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTally.Resources
{
    internal class Config
    {
        public const bool DEV_BUILD = true;
        public const string APPLICATION_NAME = "GameTally";
        public const string AUTHOR = "@repozen";

        public const string SPLASH_SCREEN_PATH = "Splash/splash.bmp";
        public const string SPLASH_FOR_DEVELOPERS_SCREEN_PATH = "Splash/DEVsplash.bmp";
        public const string NO_COVER_PATH = "Images/nocover.bmp";
        public const string TRAY_ICON_PATH = "Images/ic_launcher.ico";
        public const string SAVE_PATH = "Bin/sfxw2.cde";

        public static Size IMAGE_LIST_SIZE { get; } = new Size(170, 256);
        public struct SUPPORTED_RESOLUTIONS{
            public static Size SizeFirst { get; } = new Size(600, 900);

            public SUPPORTED_RESOLUTIONS(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public readonly int Width;
            public readonly int Height;
        }

        public const string TIME_COUNTER_FORMAT = "0h 0m";

        public const string STEAM_GRID_DB_LINK = "https://www.steamgriddb.com/grids/600x900/all/all/1";
    }
}
