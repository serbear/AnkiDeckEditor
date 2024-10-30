using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class WordFormsCopyStrategy : BaseCopyStrategy
{
    private readonly Dictionary<object, string> _tagTemplates = new()
    {
        { typeof(NonVerbWordFormCollection), FieldTags.SimpleWordItemsWithSseFormTemplate },
        { typeof(VerbWordFormCollection), FieldTags.VerbItemsTemplate }
    };


    public override string DoCopyWordForms(WordFormsCollectionBase data)
    {
        var wordForms = _tagTemplates[data.GetType()];
        var fieldIndex = 0;

        foreach (var wordForm in data.WordFormsCollection)
        {
            fieldIndex++;
            var replacement = string.IsNullOrWhiteSpace(wordForm)
                ? PublicConst.LongDashHtmlCode
                : wordForm.Trim();
            wordForms = wordForms.Replace(FieldTags.GetPlaceMarker(fieldIndex), replacement);
        }

        var result = FieldTags.SimpleWordTemplate
            .Replace(FieldTags.GetPlaceMarker(1), wordForms)
            .Replace("\n", "");

        return result;
    }
}