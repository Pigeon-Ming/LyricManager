using LyricManager.Controls;
using LyricManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LyricManager
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        JsonArray jsonArray;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAsLRCFile.IsEnabled = false;
            LoadingProgressRing.Visibility = Visibility.Visible;
            jsonArray = await OnlineMessages.MusicHelper.GetMusicJsonArrayAsync(Title_TextBox.Text);

            if (jsonArray.Count <= 0) return;

            LyricSelecterComboBox.Items.Clear();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                JsonObject obj = jsonArray[i].GetObject();
                String Title = obj["name"].GetString();
                String Artist = "";
                JsonArray array = obj["artists"].GetArray();
                for (int j = 0; j < array.Count; j++)
                {
                    JsonObject artistObj = array[j].GetObject();
                    Artist += artistObj["name"].GetString();
                }
                if (!String.IsNullOrEmpty(Artist_TextBox.Text))
                    if (Artist.IndexOf(Artist_TextBox.Text) == -1)
                        break;

                String Album = "";
                JsonObject albumObj = obj["album"].GetObject();
                Album = albumObj["name"].GetString();

                LyricSelecterComboBox.Items.Add(new LyricSelecterItemControl(Title,Artist,Album));
            }
            if (LyricSelecterComboBox.Items.Count > 0)
            {
                LyricSelecterComboBox.IsEnabled = true;
                LyricSelecterComboBox.SelectedIndex = 0;
            }
            else
            {
                LyricSelecterComboBox.IsEnabled = false;
                ContentTextBox.Text = "没有找到歌词";
                LoadingProgressRing.Visibility = Visibility.Collapsed;

            }

            

        }

        private async void LyricSelecterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LyricSelecterComboBox.Items.Count == 0) return;
            LoadingProgressRing.Visibility = Visibility.Visible;
            JsonObject jsonObject = jsonArray[LyricSelecterComboBox.SelectedIndex].GetObject();
            ContentTextBox.Text = await OnlineMessages.LyricHelper.GetLyricByMusicID(Convert.ToInt32(jsonObject["id"].GetNumber()));
            LoadingProgressRing.Visibility = Visibility.Collapsed;
            SaveAsLRCFile.IsEnabled = true;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileManager.SaveLRCFileAsync(ContentTextBox.Text);
        }
    }
}
