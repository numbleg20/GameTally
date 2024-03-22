using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTally.Helpers
{
    internal class LoadSaveHelper
    {
        public static void LoadFromFile()
        {
            if (File.Exists(Config.SAVE_PATH))
            {
                using (var stream = File.Open(Config.SAVE_PATH, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');

                            if (parts.Length >= 3)
                            {
                                var gameName = parts[0];
                                var gamePath = parts[1];
                                var gameTime = parts[2];
                                var gameCover = Config.NO_COVER_PATH;

                                if (parts.Length >= 4)
                                {
                                    gameCover = parts[3];
                                }

                                GlobalVariables.gamesList.Add(gameName, gamePath);
                                GlobalVariables.gamesTimeList.Add(gameName, gameTime);
                                GlobalVariables.gamesCoverList.Add(gameName, gameCover);
                            }
                        }
                    }
                }
            }
        }
    }
}
