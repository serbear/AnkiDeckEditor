using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class SpeechPartGovernmentCopyStrategy : BaseCopyStrategy
{
    public override string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        var selectedVerbControls = data.Where(e => (e as ToggleItem)!.IsChecked);
        var result = selectedVerbControls
            .Select(verbControl =>
                FieldTags.VerbControlItemTemplate.Replace(
                    FieldTags.GetPlaceMarker(1),
                    (verbControl as ToggleItem)?.Title))
            .Aggregate("", (current, item) => current + item);

        result = FieldTags.VerbControlTemplate.Replace(FieldTags.GetPlaceMarker(1), result);
        return result;
    }

    // public string DoCopyString(string data)
    // {
    //     throw new System.NotImplementedException();
    // }
    //
    public override string DoCopyList(List<string> data)
    {
        if (data.Count.Equals(0)) return ":: NO SPEECH PART GOVERNMENT ::";

        return ":: GOVERNMENT ::";
    }
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