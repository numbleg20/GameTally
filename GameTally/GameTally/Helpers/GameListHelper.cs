using GameTally.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GameTally.Helpers
{
    internal class GameListHelper
    {
        public static ImageList imageList = new ImageList();

        /// <summary>
        /// Change cover logic
        /// </summary>
        public static void ChangeCover(ListViewItem e)
        {
            imageList.ImageSize = Config.IMAGE_LIST_SIZE;
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|All files (*.*)|*.*";
                fd.Title = "Select an image";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = fd.FileName;
                    Image img = Image.FromFile(selectedFilePath);
                    if (ImageResolutionHelper.ImageSupported(img.Width, img.Height))
                    {
                        try
                        {
                            var gameName = GlobalVariables.gamesList.Keys.ElementAt(e.Index);

                            if (GlobalVariables.gamesCoverList.ContainsKey(gameName))
                            {
                                GlobalVariables.gamesCoverList[gameName] = selectedFilePath;
                            }
                            else
                            {
                                GlobalVariables.gamesCoverList.Add(gameName, selectedFilePath);
                            }

                            img = ResizeImage(img, imageList.ImageSize.Width, imageList.ImageSize.Height);
                            int imageIndex = imageList.Images.IndexOfKey(gameName);
                            if (imageIndex != -1)
                            {
                                imageList.Images[imageIndex] = img;
                            }
                            else
                            {
                                imageList.Images.Add(gameName, img);
                            }
                            e.ImageIndex = imageList.Images.IndexOfKey(gameName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The image must only be 600x900 in size", "Image error");
                        fd.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Add item to listview without cover
        /// </summary>
        public static void AddItemToList(ListView e)
        {
            imageList.ImageSize = Config.IMAGE_LIST_SIZE;
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "App (*.exe*)|*.exe*|All files (*.*)|*.*";
                fd.Title = "Select application";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    var gameName = Path.GetFileNameWithoutExtension(fd.FileName);
                    try
                    {
                        GlobalVariables.gamesList.Add(gameName, fd.FileName);
                        GlobalVariables.gamesTimeList.Add(gameName, Config.TIME_COUNTER_FORMAT);
                        if (GlobalVariables.gamesCoverList.ContainsKey(gameName) && GlobalVariables.gamesList.ContainsKey(gameName))
                        {
                            Image img = Image.FromFile(GlobalVariables.gamesCoverList[gameName]);
                            imageList.Images.Add(img);
                        }
                        else
                        {
                            Image img = Image.FromFile(Config.NO_COVER_PATH);
                            imageList.Images.Add(img);
                        }
                        e.Items.Add("", imageList.Images.Count - 1);
                    }
                    catch (Exception ex) {

                        MessageBox.Show(ex.Message, "Error"); 
                    }
                }
            }
            
            SaveHelper.SaveToFile();
        }

        /// <summary>
        /// Delete item from listview and other list's
        /// </summary>
        public static void DeleteItemFromList(ListView e, ListViewItem clickedItem)
        {
            if (clickedItem != null && clickedItem.Index >= 0 && clickedItem.Index < e.Items.Count)
            {
                string gameName = GlobalVariables.gamesList.Keys.ElementAt(clickedItem.Index);
                GlobalVariables.gamesList.Remove(gameName);
                GlobalVariables.gamesTimeList.Remove(gameName);

                if (clickedItem.ImageIndex >= 0 && clickedItem.ImageIndex < imageList.Images.Count)
                {
                    imageList.Images.RemoveAt(clickedItem.ImageIndex);
                }

                e.Items.Remove(clickedItem);

                Dictionary<string, string> newGamesList = new Dictionary<string, string>();
                int index = 0;
                foreach (var kvp in GlobalVariables.gamesList)
                {
                    if (index != clickedItem.Index)
                    {
                        newGamesList.Add(kvp.Key, kvp.Value);
                    }
                    index++;
                }
                GlobalVariables.gamesList.Clear();
                foreach (var kvp in newGamesList)
                {
                    GlobalVariables.gamesList.Add(kvp.Key, kvp.Value);
                }

                SaveHelper.SaveToFile();
            }
        }


        /// <summary>
        /// Update listView
        /// </summary>
        public static void UpdateList(ListView listView)
        {
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            listView.LargeImageList = imageList;
        }

        public static Image ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return result;
        }
    }
}
