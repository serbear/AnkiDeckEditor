using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class SpeechPartCopyStrategy : BaseCopyStrategy
{
    public override string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        var filtered = data.First(e => (e as ToggleItem)!.IsChecked) as SpeechPartToggleItem;
        // var result = FieldTags.SpeechPartTemplate
        // .Replace(FieldTags.GetPlaceMarker(1), filtered!.Title)
        // .Replace(FieldTags.GetPlaceMarker(2), filtered.Translation);
        // return result;
        return DoCopySpeechPart(filtered);
    }

    public override string DoCopySpeechPart(SpeechPartToggleItem speechPartToggleItem)
    {
        var result = FieldTags.SpeechPartTemplate
            .Replace(FieldTags.GetPlaceMarker(1), speechPartToggleItem!.Title)
            .Replace(FieldTags.GetPlaceMarker(2), speechPartToggleItem.Translation);

        return result;
    }
}