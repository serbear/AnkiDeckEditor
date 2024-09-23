using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class VerbGovernmentCopyStrategy : ICopyStrategy
{
    public string DoCopyCollection<T>(ObservableCollection<T> data)
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
}