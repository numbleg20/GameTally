using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTally.Helpers
{
    internal class SaveHelper
    {
        public static void SaveToFile()
        {
            using (var stream = File.Open(Config.SAVE_PATH, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    foreach (var gameName in GlobalVariables.gamesList.Keys)
                    {
                        var gamePath = GlobalVariables.gamesList[gameName];
                        var gameTime = GlobalVariables.gamesTimeList.ContainsKey(gameName) ? GlobalVariables.gamesTimeList[gameName] : Config.TIME_COUNTER_FORMAT;
                        var gameCover = GlobalVariables.gamesCoverList.ContainsKey(gameName) ? GlobalVariables.gamesCoverList[gameName] : Config.NO_COVER_PATH;

                        writer.WriteLine($"{gameName},{gamePath},{gameTime},{gameCover}");
                    }
                }
            }
        }
    }
}
