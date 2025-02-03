using System;
using System.ComponentModel;
using System.Threading.Tasks;
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

    [Obsolete("Obsolete")]
    public MainWindow()
    {
        InitializeComponent();
        _deckTypeSelectScreen = this.FindControl<DeckTypeSelectScreen>("DeckTypeSelectScreen");

        _deckTypeSelectScreen!.OnScreenChangedEvent += MyUserControlOnScreenChangedEvent;
        Closing += OnWindowClosing;

        PublicConst.MainWindowReference = this;
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

    [Obsolete("Obsolete")]
    private async void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        // Do not ask confirmation if the deck is empty.
        if (!_currentDeck!.IsCollectionEmpty) return;

        if (_currentDeck == null || _currentDeck.IsCollectionExported) return;
        e.Cancel = true;
        var requestResult = await ExportCollectionRequest();

        if (!requestResult) _currentDeck.IsCollectionExported = true;

        Close();
    }

    // todo: принажатии "выход" не спрашивает об экспорте колоды.


    [Obsolete("Obsolete")]
    private async Task<bool> ExportCollectionRequest()
    {
        var dialogResult = (bool)(await DialogHost.Show(new ExportCollectionDialog(), PublicConst.MainDialogHost))!;
        var result = false;

        if (dialogResult) result = await _currentDeck?.ExportDeck()!;

        return result;
    }
}