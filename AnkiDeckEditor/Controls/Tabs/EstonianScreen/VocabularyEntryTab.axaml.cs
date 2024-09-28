using System.Collections.ObjectModel;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

public partial class VocabularyEntryTab : UserControl
{
    public VocabularyEntryTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SpeechPartCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;

        UncheckAllCheckBoxes(ref speechPartItems);
        SwitchTabItems(ref dataContext, ref sender);
    }

    private static void UncheckAllCheckBoxes(ref ObservableCollection<SpeechPartToggleItem> toggleItems)
    {
        // Uncheck all items in the "Part of Speech" list.
        foreach (var check in toggleItems) check.IsChecked = false;
    }

    private static void SwitchTabItems(ref EstonianScreenViewModel dataContext, ref object? sender)
    {
        // Switch tabs between "word forms" and "verb forms" according to a selected speech part.
        var checkbox = (CheckBox)sender!;
        var isCheckBoxChecked = checkbox.IsChecked.Equals(true);
        var isVerbSelected = checkbox.Tag != null;

        isVerbSelected = isVerbSelected && isCheckBoxChecked;
        dataContext.IsWordFormsTabItemVisible = !isVerbSelected;
        dataContext.IsVerbFormsTabItemVisible = isVerbSelected;
    }
}