using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DynamicData;

namespace AnkiDeckEditor.Services;

/// <summary>
/// Contains functionality for manipulating with text strings.
/// </summary>
public partial class StringManipulator
{
    [GeneratedRegex(@"\s(?=\p{Po})")]
    private partial Regex OtherPunctuationAfterSpaceRegex();

    [GeneratedRegex(@"(\p{Pf}|\p{Pe})(?!\s)")]
    public partial Regex ClosePunctuationsRegex();

    [GeneratedRegex(@"(\p{Ps}|\p{Pi})\s+")]
    private partial Regex OpenPunctuationsRegex();

    [GeneratedRegex(@"(?<=</\w+>)")]
    private partial Regex CloseHtmlTag();

    [GeneratedRegex(@"\s(?=\p{Pf}|\p{Pe})")]
    private partial Regex ClosePunctuationsAfterSpaceRegex();

    public string ResultString => _resultString.TrimEnd();

    private string _resultString;

    // ReSharper disable once ConvertToPrimaryConstructor
    public StringManipulator(string targetString)
    {
        _resultString = targetString;
    }
    public StringManipulator(){}

    private static string ReplaceByRegexp(Regex generatedPattern, string value, string replaceWith)
    {
        return generatedPattern.Replace(value, replaceWith);
    }

    public StringManipulator AddSpaseAfterCloseHtmlTag()
    {
        _resultString = ReplaceByRegexp(CloseHtmlTag(), _resultString, " ");
        return this;
    }

    public StringManipulator RemoveLeftSpaceFromPunctuation()
    {
        _resultString = ReplaceByRegexp(OtherPunctuationAfterSpaceRegex(), _resultString, "");
        return this;
    }

    public StringManipulator AddSpaceAfterClosePunctuation()
    {
        _resultString = ReplaceByRegexp(ClosePunctuationsRegex(), _resultString, "$1 ");
        return this;
    }

    public StringManipulator RemoveRightSpaceClosePunctuation()
    {
        _resultString = ReplaceByRegexp(OpenPunctuationsRegex(), _resultString, "$1");
        return this;
    }

    public StringManipulator RemoveLeftSpaceClosePunctuation()
    {
        _resultString = ReplaceByRegexp(ClosePunctuationsAfterSpaceRegex(), _resultString, "");
        return this;
    }

    public StringManipulator FixDotPunctuation()
    {
        var closePunctuationChars = GetPunctuationChars(UnicodeCategory.ClosePunctuation);
        foreach (var c in closePunctuationChars)
        {
            var charSequence = $".{c}";
            if (!_resultString.Contains(charSequence)) continue;
            var swappedChars = $"{c}.";
            _resultString = _resultString.Replace(charSequence, swappedChars);
        }

        return this;
    }

    private static List<char> GetPunctuationChars(UnicodeCategory category)
    {
        var output = new List<char>();

        for (var i = 0; i <= char.MaxValue; i++)
        {
            var c = (char)i;
            if (char.GetUnicodeCategory(c) == category) output.Add(c);
        }

        return output;
    }

    // Регулярное выражение для разделения по любым символам, кроме букв
    [GeneratedRegex("([^a-zA-Zа-яА-ЯõäöüÕÄÖÜ]+)")]
    private static partial Regex SeparateLettersRegex();

    public IEnumerable<string> SeparateLetters(out List<int> punctuationIndexes)
    {
        punctuationIndexes = [];

        var result = SeparateLettersRegex()
            .Split(_resultString)
            // Remove the last empty element.
            .Where(e => e != "").ToArray();
        punctuationIndexes.AddRange(from s in result where SeparateLettersRegex().IsMatch(s) select result.IndexOf(s));

        return result;
    }
}