using GameTally.Enums;
using GameTally.Forms;
using GameTally.Helpers;
using GameTally.Models;
using GameTally.Resources;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTally
{
    public partial class MainWindow : Form
    {
        public static ListViewItem clickedItem = null;
        public MainWindow()
        {
            InitializeComponent();

            ListViewStyle.ListView_SetSpacing(listView1, 170 + 10, 256 + 10);

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
                if (MessageWindow.ShowL("Warning", "Exit the application? (all data will be saved)")) SaveHelper.SaveToFile();
                else args.Cancel = true;
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
            var changePathItem = itemContextMenu.MenuItems.Add("Change path");
            itemContextMenu.MenuItems.Add("-");
            var playGameItem = itemContextMenu.MenuItems.Add("Play");
            itemContextMenu.MenuItems.Add("-");
            var infoItem = itemContextMenu.MenuItems.Add("Details");
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
                        itemContextMenu.Show(listView, new Point(args.Location.X + 15, args.Location.Y + 15));
                    }
                }
            };

            /// <summary>
            /// Launch the game after pressing the button
            /// </summary>
            playGameItem.Click += (sender, args) =>
            {
                GameHelper.PlayGame(GlobalVariables.gamesList.ElementAt(clickedItem.Index).Value);
            };

            /// <summary>
            /// Move the program to tray if it was minimized
            /// </summary>
            Resize += (sender, args) =>
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
            deleteItem.Click += (sender, args) =>
            {
                if (MessageWindow.ShowL("Warning", "Are you sure you want to delete this item?"))
                {
                    GameListHelper.DeleteItemFromList(listView1, clickedItem);
                }
            };

            /// <summary>
            /// Change the path to the game if the previous one was damaged
            /// </summary>
            changePathItem.Click += (sender, args) =>
            {
                using (OpenFileDialog fd = new OpenFileDialog())
                {
                    fd.Filter = "App (*.exe*)|*.exe*|All files (*.*)|*.*";
                    fd.Title = "Select application";
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        var gameName = Path.GetFileNameWithoutExtension(fd.FileName);
                        try
                        {
                            GlobalVariables.gamesList[GlobalVariables.gamesList.ElementAt(clickedItem.Index).Key] = fd.FileName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                }
            };

            /// <summary>
            /// Show form with info about game
            /// </summary>
            infoItem.Click += (sender, args) =>
            {
                Info info = new Info();
                if(GlobalVariables.gamesTimeList != null)
                {
                    var gameName = "gameName";
                    try
                    {
                        var gamePath = GlobalVariables.gamesList[GlobalVariables.gamesList.ElementAt(clickedItem.Index).Key];
                        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(gamePath);
                        if (string.IsNullOrEmpty(versionInfo.ProductName)) gameName = Path.GetFileNameWithoutExtension(gamePath);
                        else gameName = versionInfo.ProductName;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
                    info.GameName.Text = gameName;
                    info.GameTime.Text = $"Time Played {GlobalVariables.gamesTimeList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)]}";
                    if (GlobalVariables.gamesCoverList.ContainsKey(GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)) &&
                        GlobalVariables.gamesCoverList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)] != null)
                    {
                        try { info.cover.Image = Image.FromFile(GlobalVariables.gamesCoverList[GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index)]); }
                        catch (Exception ex) { info.cover.Image = Image.FromFile(Config.NO_COVER_PATH); }
                    }
                    else info.cover.Image = Image.FromFile(Config.NO_COVER_PATH);
                }
                if(GlobalVariables.gamesList != null)
                {
                    Info.gamepath = $"{GlobalVariables.gamesList[GlobalVariables.gamesList.ElementAt(clickedItem.Index).Key]}";
                }
                info.ShowDialog();
            };

            /// <summary>
            /// Show info about program
            /// </summary>
            aboutToolStripMenuItem.Click += (sender, args) =>
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
            Activated += (sender, args) => MicaTitleBar(Handle);

            /// <summary>
            /// Add the following line to call MicaTitleBar when the visibility of the window changes
            /// </summary>
            VisibleChanged += (sender, args) =>
            {
                if (Visible)
                {
                    MicaTitleBar(Handle);
                }
            };

            

            /// <summary>
            /// Add program to startup
            /// </summary>
            addstartupToolStripMenuItem1.Click += (sender, args) =>
            {
                ApplicationHelper.AddToStartup();
            };

            /// <summary>
            /// Remove a program from startup
            /// </summary>
            deletestartupToolStripMenuItem.Click += (sender, args) =>
            {
                ApplicationHelper.DeleteFromStartup();
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
                /// Check whether the library is empty or not and if it is empty, display the text
                /// </summary>
                empty.Visible = listView1.Items.Count == 0;

                /// <summary>
                /// Check if the game is running and if so, change the status
                /// </summary>
                if (GameHelper.GameFromListIsRunning(GlobalVariables.gamesList))
                {
                    var gameName = "";
                    var gamePath = GlobalVariables.gamesList[GlobalVariables.currentOpenedGame];
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(gamePath);
                    if (string.IsNullOrEmpty(versionInfo.ProductName)) gameName = GlobalVariables.currentOpenedGame;
                    else gameName = versionInfo.ProductName;

                    GlobalVariables.currentUserStatus = (int)UserStatus.IN_GAME;
                    userStateLabel.Text = $"In-Game: {gameName}";
                    userStateLabel.ForeColor = ColorTranslator.FromHtml("#779a40");
                    TimeCounterHeleper.Count();
                    SaveHelper.SaveToFile();
                }
                else
                {
                    GlobalVariables.currentUserStatus = (int)UserStatus.OFFLINE;
                    userStateLabel.ForeColor = ColorTranslator.FromHtml("#333333");
                    userStateLabel.Text = $"{Convertor.UserStatusConvertor(GlobalVariables.currentUserStatus)}";
                }
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
