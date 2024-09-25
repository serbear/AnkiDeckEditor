using System.Collections.Generic;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class WordFormsCopyStrategy : ICopyStrategy
{
    private readonly Dictionary<string, string> _tagTemplates = new()
    {
        { "WordFormsAnkiField", FieldTags.SimpleWordItemsWithSseFormTemplate },
        { "VerbWordFormsAnkiField", FieldTags.VerbItemsTemplate }
    };

    public string DoCopyList(List<object> data)
    {
        // The first element is the identifier of the collection of word forms: verb or non-verb.
        // The identifier value is the tag of the button that invokes the command to copy data to the clipboard.
        var wordForms = _tagTemplates[(string)data[0]];

        var fieldIndex = 0;

        foreach (string wordForm in data)
        {
            fieldIndex++;
            var replacement = string.IsNullOrWhiteSpace(wordForm)
                ? PublicConsts.LongDashHtmlCode
                : wordForm.Trim();
            wordForms = wordForms.Replace(FieldTags.GetPlaceMarker(fieldIndex), replacement);
        }

        var result = FieldTags.SimpleWordTemplate
            .Replace(FieldTags.GetPlaceMarker(1), wordForms)
            .Replace("\n", "");

        return result;
    }
}