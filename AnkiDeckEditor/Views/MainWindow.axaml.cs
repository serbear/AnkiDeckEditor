using System;
using AnkiDeckEditor.Views.Screens;
using Avalonia.Controls;

namespace AnkiDeckEditor.Views;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindow : Window
{
    private readonly DeckTypeSelectScreen? _deckTypeSelectScreen;

    public MainWindow()
    {
        InitializeComponent();
        _deckTypeSelectScreen = this.FindControl<DeckTypeSelectScreen>("DeckTypeSelectScreen");
        _deckTypeSelectScreen.OnScreenChangedEvent += MyUserControlOnScreenChangedEvent;
    }

    private void MyUserControlOnScreenChangedEvent(object? sender, EventArgs e)
    {
        MainWindowContentArea.Content = ((ScreenEventArgs)e).Screen;
        _deckTypeSelectScreen.IsVisible = false;
    }
}