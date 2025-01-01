using System.Threading.Tasks;
using Avalonia.Controls;

namespace AnkiDeckEditor.Services;

public static class FileDialogs
{
    public static async Task<string?> GetSaveFilePath()
    {
        const string FILE_DIALOG_TITLE = "Save Collection";

        var openFileDialog = new SaveFileDialog
        {
            Title = FILE_DIALOG_TITLE,
            Filters = [new FileDialogFilter { Name = "CSV Text Files", Extensions = { "csv" } }],
            DefaultExtension = "csv"
        };

        // var filePath = await openFileDialog.ShowAsync(new Window());
        var filePath = await openFileDialog.ShowAsync(PublicConst.MainWindowReference!);

        return filePath;
    }
}