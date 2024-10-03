using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.Libs;

/// <summary>
/// Contains functionality for working with the clipboard.
/// </summary>
public static class Clipboard
{
    /// <summary>
    /// Sets a text to the clipboard.
    /// </summary>
    /// <param name="text">The text to be placed into the clipboard.</param>
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

    /// <summary>
    /// Gets a text from the clipboard.
    /// </summary>
    public static async Task<string?> Get()
    {
        if (GetWindow().Clipboard == null) return string.Empty;
        return await GetWindow().Clipboard?.GetTextAsync()!;
    }
}