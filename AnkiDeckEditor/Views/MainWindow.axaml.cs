using System;
using System.ComponentModel;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views.Screens;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AnkiDeckEditor.Views.Dialogs;
using DialogHostAvalonia;

namespace AnkiDeckEditor.Views;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindow : Window
{
    private readonly DeckTypeSelectScreen? _deckTypeSelectScreen;
    private DeckScreenViewModel? _currentDeck;

    public MainWindow()
    {
        InitializeComponent();
        _deckTypeSelectScreen = this.FindControl<DeckTypeSelectScreen>("DeckTypeSelectScreen");
        _deckTypeSelectScreen!.OnScreenChangedEvent += MyUserControlOnScreenChangedEvent;
        Closing += OnWindowClosing;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MyUserControlOnScreenChangedEvent(object? sender, EventArgs e)
    {
        _currentDeck = ((ScreenEventArgs)e).Screen.DataContext as DeckScreenViewModel;
        this.FindControl<ContentControl>("MainWindowContentArea")!.Content = ((ScreenEventArgs)e).Screen;
        _deckTypeSelectScreen!.IsVisible = false;
    }

    private async void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        if (_currentDeck == null || _currentDeck.IsCollectionExported) return;
        
        e.Cancel = true;

        var dialogResult = (bool)(await DialogHost.Show(new ExportCollectionDialog(), PublicConst.MainDialogHost))!;

        if (!dialogResult) return;

        var result = _currentDeck.ExportDeck();

        if (!result) return;

        _currentDeck.IsCollectionExported = true;
        Close();
    }
}