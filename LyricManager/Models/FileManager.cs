using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LyricManager.Models
{
    public class FileManager
    {
        public static async Task SaveLRCFileAsync(String Content)
        {

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("LRC歌词文件", new List<string>() { ".lrc" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "*.lrc";

            StorageFile file1 = await savePicker.PickSaveFileAsync();
            await Windows.Storage.FileIO.WriteTextAsync(file1, Content);
        }
    }
}
