namespace AnkiDeckEditor.Models;

public class FieldTemplateColllection
{
    public string TranslationOriginalTemplate { get; set; }

    public string SelectedEntityTemplate { get; set; }

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public string SpeechPartTemplate { get; set; }

    /// <summary>
    /// 1 - items
    /// </summary>
    public string SimpleWordTemplate { get; set; }

    public string SimpleWordItemsWithSseFormTemplate { get; set; }

    public string VerbTemplate { get; set; }

    public string VerbItemsTemplate { get; set; }

    public string VerbControlTemplate { get; set; }

    public string VerbControlItemTemplate { get; set; }
}