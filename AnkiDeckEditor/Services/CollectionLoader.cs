using System.Collections.Generic;
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
        const string FILE_PATH = "/home/sergei/Developments/AnkiDeckEditor/AnkiDeckEditor/speech_part.json";
        return JsonFileReader.Read<ObservableCollection<SpeechPartToggleItem>>(FILE_PATH);
    }

    public static Dictionary<string, string>? LoadFieldTags()
    {
        const string FILE_PATH = "/home/sergei/Developments/AnkiDeckEditor/AnkiDeckEditor/field_templates.json";
        return JsonFileReader.Read<Dictionary<string, string>>(FILE_PATH);
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