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
        public static async Task Count()
        {
            string hrsandmnts = "";
            if (GlobalVariables.gamesTimeList.TryGetValue(GlobalVariables.gamesList.Keys.ElementAt(MainWindow.clickedItem.Index), out hrsandmnts))
            {
                string[] timeComponents = hrsandmnts.Split(':');
                hours = int.Parse(timeComponents[0]);
                minutes = int.Parse(timeComponents[1]);
            }
            while (true)
            {
                minutes++;

                if (minutes == 60)
                {
                    minutes = 0;
                    hours++;
                }

                if (hours == 24)
                {
                    hours = 0;
                }
                GlobalVariables.gamesTimeList[GlobalVariables.gamesList.Keys.ElementAt(MainWindow.clickedItem.Index)] = $"{hours:D2}:{minutes:D2}";
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
