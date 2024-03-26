using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

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
            try
            {
                Process.Start(GamePath);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
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