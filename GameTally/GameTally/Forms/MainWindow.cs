using System;
using GameTally.Forms;
using GameTally.Helpers;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Windows.Forms;
using GameTally.Resources;
using GameTally.Enums;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using GameTally.Models;

namespace GameTally
{
    public partial class MainWindow : Form
    {
        public static ListViewItem clickedItem = null;
        public MainWindow()
        {
            InitializeComponent();

            /// <summary>
            /// Load all data from save file
            /// </summary>
            if (File.Exists(Config.SAVE_PATH))
            {
                LoadSaveHelper.LoadFromFile();

                GameListHelper.imageList.Images.Clear();

                foreach (var gameName in GlobalVariables.gamesList.Keys)
                {
                    var gameCover = GlobalVariables.gamesCoverList[gameName];
                    Image img = Image.FromFile(gameCover);
                    GameListHelper.imageList.Images.Add(gameName, img);
                }

                GameListHelper.imageList.ImageSize = Config.IMAGE_LIST_SIZE;
                GameListHelper.imageList.ColorDepth = ColorDepth.Depth32Bit;
                listView1.LargeImageList = GameListHelper.imageList;

                listView1.Items.Clear();

                foreach (var gameName in GlobalVariables.gamesList.Keys)
                {
                    listView1.Items.Add("", gameName);
                }
            }

            /// <summary>
            /// If the form was closed, save all data
            /// </summary>
            FormClosing += (sender, args) =>
            {
                SaveHelper.SaveToFile();
            };

            /// <summary>
            /// Operations with tray icon
            /// </summary>
            trayContextMenu.MenuItems.Add("Open", (sender, args) => {
                BringToFront();
                WindowState = FormWindowState.Normal;
                MicaTitleBar(Handle);
                ShowInTaskbar = true;
                Show();
            });
            trayContextMenu.MenuItems.Add("Exit", (sender, args) => Application.Exit());
            notifyIcon1.Icon = new Icon(Config.TRAY_ICON_PATH);
            notifyIcon1.Text = Config.APPLICATION_NAME;
            notifyIcon1.ContextMenu = trayContextMenu;
            notifyIcon1.DoubleClick += (sender, args) =>
            {
                BringToFront();
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                Show();
            };

            /// <summary>
            /// Activate dark theme for context menu
            /// </summary>
            _ = Uxtheme.SetPreferredAppMode(2);
            Uxtheme.FlushMenuThemes();

            /// <summary>
            /// Add items to context menu
            /// </summary>
            var changeCoverItem = itemContextMenu.MenuItems.Add("Change cover");
            itemContextMenu.MenuItems.Add("-");
            var playGameItem = itemContextMenu.MenuItems.Add("Play ");
            itemContextMenu.MenuItems.Add("-");
            var infoItem = itemContextMenu.MenuItems.Add("Info");
            itemContextMenu.MenuItems.Add("-");
            var deleteItem = itemContextMenu.MenuItems.Add("Remove");

            /// <summary>
            /// Change cover activation
            /// </summary>
            changeCoverItem.Click += (sender, args) =>
            {
                GameListHelper.ChangeCover(clickedItem);
                GameListHelper.UpdateList(listView1);
            };

            /// <summary>
            /// Add selected game file (.exe) to listview from openfiledialog
            /// </summary>
            addToolStripMenuItem.Click += (sender, args) =>
            {
                GameListHelper.AddItemToList(listView1);
                GameListHelper.UpdateList(listView1);
            };

            /// <summary>
            /// Show context menu if the right mouse button was pressed on one of the items from the listview
            /// </summary>
            listView1.MouseClick += (sender, args) =>
            {
                if (args.Button == MouseButtons.Right)
                {
                    ListView listView = (ListView)sender;
                    clickedItem = listView.GetItemAt(args.X, args.Y);

                    if (clickedItem != null)
                    {
                        playGameItem.Text = $"Run \"{GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)}\"";
                        itemContextMenu.Show(listView, new Point(args.Location.X + 15, args.Location.Y + 15));
                    }
                }
            };

            /// <summary>
            /// Launch the game after pressing the button
            /// </summary>
            playGameItem.Click += (sender, args) =>
            {
                string path = "";
                if (GlobalVariables.gamesList.TryGetValue(GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index), out path))
                {
                    GameHelper.PlayGame(path);
                }
            };

            /// <summary>
            /// Move the program to tray if it was minimized
            /// </summary>
            Resize += (sender, e) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.BalloonTipTitle = "GameTally";
                    notifyIcon1.BalloonTipText = "The program was minimized to tray";
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ShowBalloonTip(5000);

                    ShowInTaskbar = false;
                    Hide();
                    ApplicationHelper.HideFromAltTab(Handle);
                }
            };

            /// <summary>
            /// Delete game from list view
            /// </summary>
            deleteItem.Click += (sender, e) =>
            {
                if (MessageWindow.ShowL("Warning", "Are you sure you want to delete this item?"))
                {
                    GameListHelper.DeleteItemFromList(listView1, clickedItem);
                }
            };

            /// <summary>
            /// Show form with info about game
            /// </summary>
            infoItem.Click += (sender, e) =>
            {
                InfoWindow info = new InfoWindow();
                if(GlobalVariables.gamesTimeList != null)
                {
                    info.label1.Text = $"Time in game: {GlobalVariables.gamesTimeList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)]}";
                    if (GlobalVariables.gamesCoverList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)] != null) 
                        info.cover.Image = Image.FromFile(GlobalVariables.gamesCoverList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)]);
                    else info.cover.Image = Image.FromFile(Config.NO_COVER_PATH);
                }
                info.ShowDialog();
            };

            /// <summary>
            /// Show info about program
            /// </summary>
            aboutToolStripMenuItem.Click += (sender, e) =>
            {
                new AboutWindow().ShowDialog();
            };
            
            /// <summary>
            /// Start background thread
            /// </summary>
            Thread thread = new Thread(BackgroundThread);
            thread.IsBackground = true;
            thread.Start();

            Load += (sender, args) =>
            {
                menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuStripStyle());
            };

            /// <summary>
            /// Set Mica title bar if windows version is 11
            /// </summary>
            Activated += (sender, e) => MicaTitleBar(Handle);

            /// <summary>
            /// Add the following line to call MicaTitleBar when the visibility of the window changes
            /// </summary>
            VisibleChanged += (sender, e) =>
            {
                if (Visible)
                {
                    MicaTitleBar(Handle);
                }
            };
        }

        /// <summary>
        /// Background thread function
        /// </summary>
        async void BackgroundThread()
        {
            while (true)
            {
                /// <summary>
                /// Check if the game is running and if so, change the status
                /// </summary>
                if (GameHelper.GameFromListIsRunning(GlobalVariables.gamesList))
                {
                    GlobalVariables.currentUserStatus = (int)UserStatus.IN_GAME;

                    await TimeCounterHeleper.Count();
                }
                else
                {
                    GlobalVariables.currentUserStatus = (int)UserStatus.OFFLINE;
                }
                userStateLabel.Text = $"Status: {Convertor.UserStatusConvertor(GlobalVariables.currentUserStatus)}" +
                    $"{(GlobalVariables.currentOpenedGame != null ? GlobalVariables.currentOpenedGame : "")}" ;
                await Task.Delay(1000);
            }
        }

        static void MicaTitleBar(IntPtr handle)
        {
            if (WindowsVersionHelper.IsWindows11())
            {
                var preference = Convert.ToInt32(true);
                dwmapi.DwmSetWindowAttribute(handle,
                                      dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                                      ref preference, sizeof(uint));
            }
        }
    }
}
