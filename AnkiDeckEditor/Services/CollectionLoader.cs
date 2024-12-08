using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services;

public static class CollectionLoader
{
    private const string APPLICATION_NAME = "AnkiDeckEditor";
    private const string SPEECH_PART_GOVERNMENT_JSON = "speech_part_government.json";
    private const string SPEECH_PART_JSON = "speech_part.json";
    private const string FIELD_TEMPLATES_JSON = "field_templates.json";

    public static ObservableCollection<ToggleItem> LoadVerbControls()
    {
        var collectionArray = JsonFileReader.Read<string[]>(
            Path.ConfigPath(APPLICATION_NAME, SPEECH_PART_GOVERNMENT_JSON));

        if (collectionArray is { Length: 0 })
        {
            // todo: Collection is empty. exception?
        }

        var output = new ObservableCollection<ToggleItem>(
            collectionArray!.Select(vc => new ToggleItem(vc.Trim(), false)).ToList());

        return new ObservableCollection<ToggleItem>(output.OrderBy(e => e.Title));
    }

    public static ObservableCollection<SpeechPartToggleItem>? LoadSpeechParts()
    {
        return JsonFileReader.Read<ObservableCollection<SpeechPartToggleItem>>(
            Path.ConfigPath(APPLICATION_NAME, SPEECH_PART_JSON));
    }

    public static Dictionary<string, string>? LoadFieldTags()
    {
        return JsonFileReader.Read<Dictionary<string, string>>(
            Path.ConfigPath(APPLICATION_NAME, FIELD_TEMPLATES_JSON));
    }
}

public struct FieldTags
{
    public const string CompoundVerbMarker = "<sup>üv</sup>";
    public const string VerbTemplate = "<div class=\"word-forms-container\">{1}</div>";

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