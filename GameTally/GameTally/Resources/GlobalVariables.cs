using System;
using System.Collections.Generic;
using GameTally.Enums;

namespace GameTally.Resources
{
    public class GlobalVariables
    {
        public static int currentUserStatus = (int)UserStatus.OFFLINE;
        public static string currentOpenedGame = null;
        public static Dictionary<string, string> gamesList = new Dictionary<string, string>();
        public static Dictionary<string, string> gamesTimeList = new Dictionary<string, string>();
        public static Dictionary<string, string> gamesCoverList = new Dictionary<string, string>();
    }
}