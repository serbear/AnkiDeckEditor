using System.Globalization;

namespace AnkiDeckEditor.Libs;

public static class StringHelper
{
    public static void UpperCase(ref string[] strings)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;

        for (var i = 0; i < strings.Length; i++)
            strings[i] = textInfo.ToTitleCase(strings[i]);
    }
}