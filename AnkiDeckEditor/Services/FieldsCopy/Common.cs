using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AnkiDeckEditor.Models;
using DynamicData;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Includes the common code for the copy strategies.
/// </summary>
public static class Common
{
    private const string SEPARATOR = " ";

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
        var words = data.Item1.Split(SEPARATOR);
        List<string> resultBuilder = [..words];

        // If there are no highlighted words, return the string unchanged.
        if (!data.Item2.Count.Equals(0))
            resultBuilder = words.Select((t, i) => MarkWord(t, data.Item2.Contains(i + 1))).ToList();

        return JoinWordCollection(resultBuilder);
    }

    private static string MarkWord(string value, bool isMarked)
    {
        var result = value;

        if (!isMarked) return $"{result}";

        // Only the word without punctuation marks is placed in the marker tag of the selected word.
        var separatedWords = new StringManipulator(value).SeparateLetters(out var indexes).ToList();

        for (var i = 0; i < separatedWords.Count; i++)
            if (indexes.Contains(i + 1))
                separatedWords[i] = FieldTags.SelectedEntityTemplate.Replace(
                    FieldTags.GetPlaceMarker(1), separatedWords[i]);

        result = string.Join("", separatedWords);

        return $"{result}";
    }

    private static string JoinWordCollection(IEnumerable<string> resultBuilder)
    {
        var sm = new StringManipulator(string.Join(SEPARATOR, resultBuilder).Trim())
            .AddSpaseAfterCloseHtmlTag()
            .RemoveLeftSpaceFromPunctuation()
            .AddSpaceAfterClosePunctuation()
            .RemoveRightSpaceClosePunctuation()
            .RemoveLeftSpaceClosePunctuation();
        return FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);
    }
}