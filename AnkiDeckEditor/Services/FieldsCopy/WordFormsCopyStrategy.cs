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

    // public string DoCopyCollection<T>(ObservableCollection<T> data)
    // {
    // throw new System.NotImplementedException();
    // }

    // public string DoCopyString(string data)
    // {
    // throw new System.NotImplementedException();
    // }

    // public string DoCopyList(List<string> data)
    // {
    // throw new System.NotImplementedException();
    // }

    // public string DoCopyValueTuple((string, List<int>) data)
    // {
    // throw new System.NotImplementedException();
    // }

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