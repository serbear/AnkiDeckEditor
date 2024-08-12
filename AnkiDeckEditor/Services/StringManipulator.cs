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

    public string ResultString { get; private set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public StringManipulator(string targetString)
    {
        ResultString = targetString;
    }

    private static string ReplaceByRegexp(Regex generatedPattern, string value, string replaceWith)
    {
        return generatedPattern.Replace(value, replaceWith);
    }

    public StringManipulator AddSpaseAfterCloseHtmlTag()
    {
        ResultString = ReplaceByRegexp(CloseHtmlTag(), ResultString, " ");
        return this;
    }

    public StringManipulator RemoveLeftSpaceFromPunctuation()
    {
        ResultString = ReplaceByRegexp(OtherPunctuationAfterSpaceRegex(), ResultString, "");
        return this;
    }

    public StringManipulator AddSpaceAfterClosePunctuation()
    {
        ResultString = ReplaceByRegexp(ClosePunctuationsRegex(), ResultString, "$1 ");
        return this;
    }

    public StringManipulator RemoveRightSpaceClosePunctuation()
    {
        ResultString = ReplaceByRegexp(OpenPunctuationsRegex(), ResultString, "$1");
        return this;
    }

    public StringManipulator RemoveLeftSpaceClosePunctuation()
    {
        ResultString = ReplaceByRegexp(ClosePunctuationsAfterSpaceRegex(), ResultString, "");
        return this;
    }
}