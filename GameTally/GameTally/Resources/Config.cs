using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public const string SAVE_PATH = "Bin/sfxw2.bin";

        public static Size IMAGE_LIST_SIZE { get; } = new Size(256, 119);

        public const string STEAM_GRID_DB_LINK = "https://www.steamgriddb.com/grids/460x215,920x430/all/all/1";
    }
}
