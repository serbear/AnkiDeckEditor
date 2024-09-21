using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Includes the common code for the copy strategies.
/// </summary>
public static class Common
{
    public static string ProcessCollection<T>(ObservableCollection<T> translationData)
    {
        var resultBuilder = new List<string>();

        foreach (var item in translationData)
            // Marked to learn entity.
            if ((item as ToggleItem)!.IsChecked)
            {
                var tagged =
                    FieldTags.SelectedEntityTemplate.Replace(FieldTags.GetPlaceMarker(1), (item as ToggleItem)!.Title);
                resultBuilder.Add($"{tagged}");
            }
            // A common word or punctuation.
            else
            {
                resultBuilder.Add($"{(item as ToggleItem)!.Title} ");
            }

        var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
            .AddSpaseAfterCloseHtmlTag()
            .RemoveLeftSpaceFromPunctuation()
            .AddSpaceAfterClosePunctuation()
            .RemoveRightSpaceClosePunctuation()
            .RemoveLeftSpaceClosePunctuation();
        var result = FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);

        return result;
    }
}