using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class SpeechPartCopyStrategy : ICopyStrategy
{
    public string DoCopy<T>(ObservableCollection<T> data)
    {
        var filtered = data.First(e => (e as ToggleItem)!.IsChecked) as SpeechPartToggleItem;
        var result = FieldTags.SpeechPartTemplate
            .Replace(FieldTags.GetPlaceMarker(1), filtered!.Title)
            .Replace(FieldTags.GetPlaceMarker(2), filtered.Translation);

        return result;
    }
}