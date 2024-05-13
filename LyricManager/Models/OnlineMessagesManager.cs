﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace LyricManager.Models
{
    public class OnlineMessages
    {
        public class LyricHelper
        {
            public static async Task<String> GetLyricByMusicID(int ID)
            {
                String LyricJsonString = await ResponseManager.GetString("http://music.163.com/api/song/media?id=" + ID);
                JsonObject LyricJson = JsonObject.Parse(LyricJsonString);
                return LyricJson["lyric"].GetString();
            }
        }

        public class MusicHelper
        {
            public static async Task<JsonArray> GetMusicJsonArrayAsync(String Title)
            {
                String MusicListJson = await ResponseManager.PostString("http://music.163.com/api/search/pc", new Dictionary<string, string> { ["s"] = Title, ["type"] = "1" });
                Debug.WriteLine(MusicListJson);

                JsonObject jsonObject = JsonObject.Parse(MusicListJson);
                jsonObject = jsonObject["result"].GetObject();
                JsonArray jsonArray = jsonObject["songs"].GetArray();
                return jsonArray;
            }
        }
    }
}
