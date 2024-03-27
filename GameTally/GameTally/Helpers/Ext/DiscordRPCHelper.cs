using DiscordRPC.Logging;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTally.Resources;

namespace GameTally.Helpers.Ext
{
    internal class DiscordRPCHelper
    {
        public static DiscordRpcClient client;
        static string _userState = "Idle";
        static string _gameName = " ";
        public static void RPCInitialize()
        {
            client = new DiscordRpcClient(Config.RPC_CLIENT_KEY);
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = _userState,
                State = _gameName,
                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                    LargeImageText = "GameTally",
                }
            });
        }

        public static void RPCStart(string gameName)
        {
            _userState = "Plying";
            _gameName = gameName;
            UpdatePresence();
            client.Invoke();
        }
        public static void UpdateToIdle()
        {
            _userState = "Idle";
            _gameName = " ";
            UpdatePresence();
        }
        public static void RPCStop()
        {
            UpdatePresence();
            client.Dispose();
        }

        private static void UpdatePresence()
        {
            client.SetPresence(new RichPresence()
            {
                Details = _userState,
                State = _gameName,
                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                    LargeImageText = "GameTally",
                }
            });
        }
    }
}
