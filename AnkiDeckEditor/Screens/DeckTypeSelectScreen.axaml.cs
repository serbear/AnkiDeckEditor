using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnkiDeckEditor.Screens;

public partial class DeckTypeSelectScreen : UserControl
{
    public DeckTypeSelectScreen()
    {
        InitializeComponent();
    }

    public event EventHandler? OnScreenChangedEvent;

    private void ShowEstonianScreenButton_OnClick(
        object sender,
        RoutedEventArgs e)
    {
        var screen = new ScreenEventArgs { Screen = new EstonianScreen() };
        OnScreenChangedEvent?.Invoke(this, screen);
    }

    private void ShowEnglishScreenButton_OnClick(
        object sender,
        RoutedEventArgs e)
    {
        var screen = new ScreenEventArgs { Screen = new EnglishScreen() };
        OnScreenChangedEvent?.Invoke(this, screen);
    }
}

public class ScreenEventArgs : EventArgs
{
    public required UserControl Screen { get; set; }
}