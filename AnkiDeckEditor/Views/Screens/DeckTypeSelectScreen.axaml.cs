using System;
using AnkiDeckEditor.Libs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Views.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class DeckTypeSelectScreen : UserControl
{
    public DeckTypeSelectScreen()
    {
        InitializeComponent();
        SizeChanged += OnWindowSizeChanged;
    }

    private void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var offset = Common.GetGoldenGoldenRatioOffset(e.NewSize.Height);
        this.FindControl<StackPanel>("DeckTypeSelectionStackPanel")!.Margin = new Thickness(0, 0, 0, offset);
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