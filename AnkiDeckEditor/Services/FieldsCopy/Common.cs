using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Includes the common code for the copy strategies.
/// </summary>
public static class Common
{
    public static string ProcessCollection<T>(ObservableCollection<T> translationData)
    {
        var resultBuilder = translationData.Select(
                item => MarkWord((item as ToggleItem)!.Title!, (item as ToggleItem)!.IsChecked))
            .ToList();
        // var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
        // .AddSpaseAfterCloseHtmlTag()
        // .RemoveLeftSpaceFromPunctuation()
        // .AddSpaceAfterClosePunctuation()
        // .RemoveRightSpaceClosePunctuation()
        // .RemoveLeftSpaceClosePunctuation();
        // var result = FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);

        return JoinWordCollection(resultBuilder);
        // return result;
    }

    public static string ProcessTuple(ValueTuple<string, List<int>> data)
    {
        var words = data.Item1.Split(" ");
        var resultBuilder = words.Select((t, i) => MarkWord(t, data.Item2.Contains(i))).ToList();


        // var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
        // .AddSpaseAfterCloseHtmlTag()
        // .RemoveLeftSpaceFromPunctuation()
        // .AddSpaceAfterClosePunctuation()
        // .RemoveRightSpaceClosePunctuation()
        // .RemoveLeftSpaceClosePunctuation();
        // var result = FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);

        return JoinWordCollection(resultBuilder);
        // return result;
    }

    private static string MarkWord(string value, bool isMarked)
    {
        var result = "";
        if (isMarked) result = FieldTags.SelectedEntityTemplate.Replace(FieldTags.GetPlaceMarker(1), value);
        return $"{result} ";
    }

    private static string JoinWordCollection(List<string> resultBuilder)
    {
        var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
            .AddSpaseAfterCloseHtmlTag()
            .RemoveLeftSpaceFromPunctuation()
            .AddSpaceAfterClosePunctuation()
            .RemoveRightSpaceClosePunctuation()
            .RemoveLeftSpaceClosePunctuation();
        return FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);
    }
}