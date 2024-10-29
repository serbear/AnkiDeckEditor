using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class SpeechPartCopyStrategy : BaseCopyStrategy
{
    public override string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        var filtered = data.First(e => (e as ToggleItem)!.IsChecked) as SpeechPartToggleItem;
        var result = FieldTags.SpeechPartTemplate
            .Replace(FieldTags.GetPlaceMarker(1), filtered!.Title)
            .Replace(FieldTags.GetPlaceMarker(2), filtered.Translation);

        return result;
    }

    // public string DoCopyString(string data)
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // public string DoCopyList(List<string> data)
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // public string DoCopyValueTuple((string, List<int>) data)
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    // public string DoCopyWordForms(WordFormsCollectionBase data)
    // {
    //     throw new System.NotImplementedException();
    // }
}