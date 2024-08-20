using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AnkiDeckEditor.Services;

public partial class StringManipulator
{
    [GeneratedRegex(@"\s(?=\p{Po})")]
    private partial Regex OtherPunctuationAfterSpaceRegex();

    [GeneratedRegex(@"(\p{Pf}|\p{Pe})(?!\s)")]
    private partial Regex ClosePunctuationsRegex();

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
}