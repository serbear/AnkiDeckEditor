using System.Collections.Generic;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class WordFormsCopyStrategy : ICopyStrategy
{
    private readonly Dictionary<object, string> _tagTemplates = new()
    {
        { typeof(NonVerbWordFormCollection), FieldTags.SimpleWordItemsWithSseFormTemplate },
        { typeof(VerbWordFormCollection), FieldTags.VerbItemsTemplate }
    };

    public string DoCopyWordForms(WordFormsCollectionBase data)
    {
        var wordForms = _tagTemplates[data.GetType()];
        var fieldIndex = 0;

        foreach (var wordForm in data.WordFormsCollection)
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