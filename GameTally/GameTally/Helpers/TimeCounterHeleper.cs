using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameTally.Helpers
{
    internal class TimeCounterHeleper
    {
        private static int hours = 0;
        private static int minutes = 0;
        private static int seconds = 0;

        public static async void Count()
        {
            string hrsandmnts = "";
            if (MainWindow.clickedItem != null && GlobalVariables.gamesTimeList.TryGetValue(GlobalVariables.gamesList.Keys.ElementAt(MainWindow.clickedItem.Index), out hrsandmnts))
            {
                string[] timeComponents = hrsandmnts.Split(' ');

                if (timeComponents.Length == 2)
                {
                    if (timeComponents[0].EndsWith("h"))
                    {
                        hours = int.Parse(timeComponents[0].Replace("h", ""));
                    }

                    if (timeComponents[1].EndsWith("m"))
                    {
                        minutes = int.Parse(timeComponents[1].Replace("m", ""));
                    }
                }
            }

            seconds++;

            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            if (minutes == 60)
            {
                minutes = 0;
                hours++;
            }

            if (hours == 24)
            {
                hours = 0;
            }
            GlobalVariables.gamesTimeList[GlobalVariables.gamesList.Keys.ElementAt(MainWindow.clickedItem.Index)] = $"{hours}h {minutes}m";
            await Task.Delay(1000).ConfigureAwait(true);
        }
    }
}
