using GameTally.Forms;
using GameTally.Resources;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameTally
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /// <summary>
            /// If the program is launched with the argument "-nosplash" then the splash screen is skipped
            /// </summary>
            bool skipSplashScreen = Environment.GetCommandLineArgs().Any(arg => arg.EndsWith("-nospash"));

            if (skipSplashScreen) Application.Run(new MainWindow());
            else
            {
                var splashScreen = new SplashScreen();
                Thread splashThread = new Thread(() =>
                {
                    splashScreen.BackgroundImage = new Bitmap((Config.DEV_BUILD ? Config.SPLASH_FOR_DEVELOPERS_SCREEN_PATH : Config.SPLASH_SCREEN_PATH));
                    Application.Run(splashScreen);
                });
                splashThread.Start();
                Thread.Sleep(new Random().Next(2000, 4000));
                splashThread.Abort();
                splashScreen.Close();
                Application.Run(new MainWindow());
            }

        }
    }
}
