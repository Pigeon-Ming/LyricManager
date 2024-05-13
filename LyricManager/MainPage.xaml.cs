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
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            JsonArray jsonArray = await OnlineMessages.MusicHelper.GetMusicJsonArrayAsync(Title_TextBox.Text);

            if (jsonArray.Count <= 0) return;
            
            for(int i = 0; i < jsonArray.Count; i++)
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

                String Album = "";
                JsonObject albumObj = obj["album"].GetObject();
                Album = albumObj["name"].GetString();

                LyricSelecterComboBox.Items.Add(new LyricSelecterItemControl(Title,Artist,Album));
            }
            LyricSelecterComboBox.SelectedIndex = 0;

            JsonObject jsonObject = jsonArray[0].GetObject();
            ContentTextBox.Text = await OnlineMessages.LyricHelper.GetLyricByMusicID(Convert.ToInt32(jsonObject["id"].GetNumber()));

        }
    }
}
