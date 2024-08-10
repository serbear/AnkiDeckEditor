using System.Collections.ObjectModel;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnkiDeckEditor.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        DataContext = new EstonianScreenViewModel();
    }

    private void ToggleButton_OnIsCheckedChanged(
        object? sender,
        RoutedEventArgs e)
    {
        // Uncheck all items in the "Part of Speech" list.
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;
        foreach (var check in speechPartItems) check.IsChecked = false;
    }

    private void WordForWordTextBox_OnTextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        var splitted = textBox.Text?.Trim().Split(" ");

        UpdateEntityContextCollection(
            ((EstonianScreenViewModel)DataContext!)
            .EntityContextCollections[textBox.Name],
            splitted);
    }

    private static void UpdateEntityContextCollection<T>(
        T collection,
        string[]? contextWords)
        where T : ObservableCollection<ToggleItem>
    {
        if (contextWords == null) return;
        collection.Clear();
        foreach (var s in contextWords)
            collection.Add(new ToggleItem(s, false));
    }
}