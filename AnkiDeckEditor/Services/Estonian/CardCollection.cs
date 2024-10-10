using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;
using Avalonia.Controls;
using DynamicData;

namespace AnkiDeckEditor.Services.Estonian;

public static class CardCollection
{
    public static void RemoveManyItems<T>(ref ObservableCollection<T> collection) where T : VocabularyCardRecord
    {
        var items = collection.Where(c => c.IsChecked.Equals(true));
        collection.RemoveMany(items);
    }

    public static void RemoveOneItem<T>(DataGrid? dataGrid, ref ObservableCollection<T> collection)
        where T : VocabularyCardRecord
    {
        var selectedItem = dataGrid?.SelectedItem as T;
        collection.Remove(selectedItem!);
    }

    public static bool AreManyCardsChecked<T>(ref ObservableCollection<T> collection)
        where T : VocabularyCardRecord
    {
        return collection.Any(c => c.IsChecked.Equals(true));
    }

    public static void ClearCollection<T>(ref ObservableCollection<T> collection)
        where T : VocabularyCardRecord
    {
        collection.Clear();
    }
}