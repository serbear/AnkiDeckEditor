using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Implement functionality for copying the Literal Translation field data into the clipboard.
/// </summary>
public class LiteralTranslationCopyStrategy : ICopyStrategy
{
    public string DoCopy<T>(ObservableCollection<T> translationData)
    {
        var result = Common.ProcessCollection(translationData);
        //     if (item.IsChecked)
        //     {
        //         var tagged = FieldTags.SelectedEntityTemplate.Replace(FieldTags.GetPlaceMarker(1), item.Title);
        //         var isCompoundVerbSelected = SpeechPartItems.Any(
        //             e => e is { VerbType: VerbTypes.Compound, IsChecked: true });
        //         var resultString =
        //             totalMarkedEntities == 1 && isCompoundVerbSelected
        //                 ? $"{tagged}{FieldTags.CompoundVerbMarker}"
        //                 : $"{tagged}";
        //
        //         resultBuilder.Add(resultString);
        //         totalMarkedEntities--;
        //     }
        result = new StringManipulator(result).FixDotPunctuation().ResultString;
        return result;
    }
}