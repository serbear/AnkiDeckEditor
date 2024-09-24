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
        // Первый элемент - идентификатор коллекции форм слова: глагол или не глагол.
        // Значением идентификатора является тег кнопки, которая вызывает команду копирования данных в буфер обмена.
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