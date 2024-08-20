using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.Libs;

public static class Clipboard
{
    public static void Set(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        var window = GetWindow();
        window.Clipboard?.SetTextAsync(text);
    }

    private static Window GetWindow()
    {
        Window output;
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: { } window
            })
            output = window;
        else
            // linux
            throw new NotImplementedException();
        return output;
    }

    public static async Task<string?> Get()
    {
        var window = GetWindow();
        var output = await window.Clipboard?.GetTextAsync()!;

        return output;
    }
}