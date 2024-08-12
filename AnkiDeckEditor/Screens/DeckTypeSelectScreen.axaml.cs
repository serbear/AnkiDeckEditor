using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class DeckTypeSelectScreen : UserControl
{
    public DeckTypeSelectScreen()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public event EventHandler? OnScreenChangedEvent;

    private void ShowEstonianScreenButton_OnClick(object sender, RoutedEventArgs e)
    {
        var screen = new ScreenEventArgs { Screen = new EstonianScreen() };
        OnScreenChangedEvent?.Invoke(this, screen);
    }

    private void ShowEnglishScreenButton_OnClick(object sender, RoutedEventArgs e)
    {
        var screen = new ScreenEventArgs { Screen = new EnglishScreen() };
        OnScreenChangedEvent?.Invoke(this, screen);
    }
}

public class ScreenEventArgs : EventArgs
{
    public required UserControl Screen { get; set; }
}