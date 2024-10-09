using AnkiDeckEditor.Models;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

// ReSharper disable once PartialTypeWithSinglePart
public partial class CardListTab : UserControl
{
    public CardListTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as EstonianScreenViewModel)?.EditCardListEntry(
            ((DataGrid)sender!).SelectedItem as EstonianCardRecord
        );
    }

    private void CardCollectionDataGrid_OnTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as EstonianScreenViewModel)?.UpdateIsRemoveCardButtonEnabledFlag();
    }
}