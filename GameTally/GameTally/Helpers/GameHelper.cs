using GameTally.Resources;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameTally.Helpers
{
    public class GameHelper
    {
        static bool GameIsRunning(string gameName)
        {
            return Process.GetProcessesByName(gameName).Length > 0;
        }

        public static void PlayGame(string GamePath)
        {
            Process.Start(GamePath);
        }

        public static bool GameFromListIsRunning(Dictionary<string, string> gamesList)
        {
            if (gamesList != null)
            {
                foreach (var game in gamesList.Keys)
                {
                    if (GameIsRunning(game))
                    {
                        GlobalVariables.currentOpenedGame = game;
                        return true;
                    }
                    else GlobalVariables.currentOpenedGame = null;
                }
            }

            return false;
        }
    }
}