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
                PublicConst.EstonianDeckTemplates["VerbControlItemTemplate"].Replace(
                    FieldTags.GetPlaceMarker(1),
                    (verbControl as ToggleItem)?.Title))
            .Aggregate("", (current, item) => current + item);

        result = PublicConst.EstonianDeckTemplates["VerbControlTemplate"].Replace(
            FieldTags.GetPlaceMarker(1),
            result);
        return result;
    }

    public override string DoCopyList(List<string> data)
    {
        if (data.Count.Equals(0)) return "";

        var result = data
            .Select(verbControl =>
                PublicConst.EstonianDeckTemplates["VerbControlItemTemplate"].Replace(
                    FieldTags.GetPlaceMarker(1),
                    verbControl))
            .Aggregate("", (current, item) => current + item);

        result = PublicConst.EstonianDeckTemplates["VerbControlTemplate"].Replace(
            FieldTags.GetPlaceMarker(1),
            result);
        return result;
    }
}