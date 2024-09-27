using System;
using AnkiDeckEditor.Views.Screens;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Views;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindow : Window
{
    private readonly DeckTypeSelectScreen? _deckTypeSelectScreen;

    public MainWindow()
    {
        InitializeComponent();
        _deckTypeSelectScreen = this.FindControl<DeckTypeSelectScreen>("DeckTypeSelectScreen");
        _deckTypeSelectScreen!.OnScreenChangedEvent += MyUserControlOnScreenChangedEvent;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MyUserControlOnScreenChangedEvent(object? sender, EventArgs e)
    {
        this.FindControl<ContentControl>("MainWindowContentArea")!.Content = ((ScreenEventArgs)e).Screen;
        _deckTypeSelectScreen!.IsVisible = false;
    }
}