using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services;

// todo: где хранить коллекции? 
public static class CollectionLoader
{
    public static ObservableCollection<ToggleItem> LoadVerbControls()
    {
        const string FILE_PATH = "/home/sergei/Developments/AnkiDeckEditor/AnkiDeckEditor/speech_part_government.json";
        var collectionArray = JsonFileReader.Read<string[]>(FILE_PATH);

        if (collectionArray.Length == 0)
        {
            // todo: Collection is empty. exception?
        }

        var output = new ObservableCollection<ToggleItem>(
            collectionArray.Select(vc => new ToggleItem(vc.Trim(), false)).ToList());

        return new ObservableCollection<ToggleItem>(output.OrderBy(e => e.Title));
    }

    public static ObservableCollection<SpeechPartToggleItem>? LoadSpeechParts()
    {
        const string FILE_PATH = "/home/sergei/Developments/AnkiDeckEditor/AnkiDeckEditor/speech_part.json";
        return JsonFileReader.Read<ObservableCollection<SpeechPartToggleItem>>(FILE_PATH);
    }
    
    
}

public struct FieldTags
{
    public const string CompoundVerbMarker = "<sup>üv</sup>";
    public const string TranslationOriginalTemplate = "<div class=\"sentence\">{1}</div>";

    public const string SelectedEntityTemplate = "<span>{1}</span>";

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public const string SpeechPartTemplate = "{1} <span>▪️</span> {2}";

    /// <summary>
    /// 1 - items
    /// </summary>
    public const string SimpleWordTemplate = "<div class=\"word-forms-container\">{1}</div>";

    public const string SimpleWordItemsWithSseFormTemplate =
        """
        <div class="grid-item amount">AIN.</div>
        <div class="grid-item form">{1}</div>
        <div class="grid-item form">{2}</div>
        <div class="grid-item form">{3}</div>
        <div class="grid-item amount">MIT.</div>
        <div class="grid-item form">{4}</div>
        <div class="grid-item form">{5}</div>
        <div class="grid-item form">{6}</div>
        <div class="grid-item short-into">L.SSE.</div>
        <div class="grid-item form-short-into">{7}</div>
        """;

    public const string VerbTemplate = "<div class=\"word-forms-container\">{1}</div>";

    public const string VerbItemsTemplate =
        "<div class=\"grid-item declension\">MA</div>\n" +
        "<div class=\"grid-item form\">{1}</div>\n" +
        "<div class=\"grid-item declension\">3P.MIN</div>\n" +
        "<div class=\"grid-item form\">{5}</div>\n" +
        "<div class=\"grid-item declension\">DA</div>\n" +
        "<div class=\"grid-item form\">{2}</div>\n" +
        "<div class=\"grid-item declension\">NUD</div>\n" +
        "<div class=\"grid-item form\">{6}</div>\n" +
        "<div class=\"grid-item declension\">3P</div>\n" +
        "<div class=\"grid-item form\">{3}</div>\n" +
        "<div class=\"grid-item declension\">2P.KÄS</div>\n" +
        "<div class=\"grid-item form\">{7}</div>\n" +
        "<div class=\"grid-item declension\">TUD</div>\n" +
        "<div class=\"grid-item form\">{4}</div>\n" +
        "<div class=\"grid-item declension\">AKSE</div>\n" +
        "<div class=\"grid-item form\">{8}</div>";

    public const string VerbControlTemplate = "<div class=\"verb-control-container\">{1}</div>";

    public const string VerbControlItemTemplate = "<span>{1}</span>";

    /// <summary>
    /// Returns the string representation of the replacement marker value in the templates with the location number.
    /// </summary>
    /// <param name="number">The place number.</param>
    public static string GetPlaceMarker(int number)
    {
        var output = $"{{{number}}}";
        return output;
    }
}