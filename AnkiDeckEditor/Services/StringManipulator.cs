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

    private string _targetString;
    public string ResultString => _targetString;

    // ReSharper disable once ConvertToPrimaryConstructor
    public StringManipulator(string targetString)
    {
        _targetString = targetString;
    }

    private string ReplaceByRegexp(Regex generatedPattern, string value, string replaceWith)
    {
        return generatedPattern.Replace(value, replaceWith);
    }


    public StringManipulator AddSpaseAfterCloseHtmlTag()
    {
        // return CloseHtmlTag().Replace(value, " ");
        _targetString = ReplaceByRegexp(CloseHtmlTag(), _targetString, " ");
        return this;
    }

    public StringManipulator RemoveLeftSpaceFromPunctuation()
    {
        // return OtherPunctuationAfterSpaceRegex().Replace(value, "");
        _targetString = ReplaceByRegexp(OtherPunctuationAfterSpaceRegex(), _targetString, "");
        return this;
    }

    public StringManipulator AddSpaceAfterClosePunctuation()
    {
        // return ClosePunctuationsRegex().Replace(value, "$1 ");
        _targetString = ReplaceByRegexp(ClosePunctuationsRegex(), _targetString, "$1 ");
        return this;
    }

    public StringManipulator RemoveRightSpaceClosePunctuation()
    {
        // return OpenPunctuationsRegex().Replace(value, "$1");
        _targetString = ReplaceByRegexp(OpenPunctuationsRegex(), _targetString, "$1");
        return this;
    }

    public StringManipulator RemoveLeftSpaceClosePunctuation()
    {
        // return ClosePunctuationsAfterSpaceRegex().Replace(value, "");
        _targetString = ReplaceByRegexp(ClosePunctuationsAfterSpaceRegex(), _targetString, "");
        return this;
    }
}