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

        // Separate words from punctuation marks.

        List<string> resultBuilder = [];

        foreach (var word in words)
        {
            var separatedWords = new StringManipulator(word).SeparateLetters(out _).ToList();
            resultBuilder.AddRange(separatedWords);
        }

        // Mark word by tag

        foreach (var i in data.Item2)
        {
            resultBuilder[i] = FieldTags.SelectedEntityTemplate.Replace(
                FieldTags.GetPlaceMarker(1), 
                resultBuilder[i]); 
        }

        // var x = new StringManipulator().ClosePunctuationsRegex();
        
        // var result = string.Join(" ", resultBuilder);

        // result = new StringManipulator(result).RemoveLeftSpaceClosePunctuation().ResultString;
        // result = new StringManipulator(result).RemoveRightSpaceClosePunctuation().ResultString;
        // result = new StringManipulator(result).RemoveLeftSpaceFromPunctuation().ResultString;
        
        // List<string> resultBuilder = [..words];

        // If there are no highlighted words, return the string unchanged.
        // if (!data.Item2.Count.Equals(0))
            // for (var i = 0; i < words.Length; i++)
                // resultBuilder[i] = MarkWord(words[i], data.Item2.Contains(i));

        // resultBuilder = words.Select((t, i) => MarkWord(t, data.Item2.Contains(i))).ToList();

        return JoinWordCollection(resultBuilder);
    }

    private static string MarkWord(string value, bool isMarked)
    {
        var result = value;

        if (!isMarked) return $"{result}";

        // Only the word without punctuation marks is placed in the marker tag of the selected word.

        var separatedWords = new StringManipulator(value).SeparateLetters(out var punctuationIndexes).ToList();

        for (var i = 0; i < separatedWords.Count; i++)

            // indexes - punctuations, not words.

            // If this is a word, not a punctuation mark.
            if (!punctuationIndexes.Contains(i))
                separatedWords[i] = FieldTags.SelectedEntityTemplate.Replace(
                    FieldTags.GetPlaceMarker(1), separatedWords[i]);

        result = string.Join("", separatedWords);

        return $"{result}";
    }

    private static string JoinWordCollection(IEnumerable<string> resultBuilder)
    {
        var sm = new StringManipulator(string.Join(SEPARATOR, resultBuilder).Trim())
            // .AddSpaseAfterCloseHtmlTag()
            .RemoveLeftSpaceFromPunctuation()
            .AddSpaceAfterClosePunctuation()
            .RemoveRightSpaceClosePunctuation()
            .RemoveLeftSpaceClosePunctuation();
        return FieldTags.TranslationOriginalTemplate.Replace(FieldTags.GetPlaceMarker(1), sm.ResultString);
    }
}