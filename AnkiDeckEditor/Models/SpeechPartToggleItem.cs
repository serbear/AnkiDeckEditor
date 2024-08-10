namespace AnkiDeckEditor.Models;

public class SpeechPartToggleItem : ToggleItem
{
    public string? Translation { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public SpeechPartToggleItem(
        string title, string? translation, bool isChecked)
        : base(title, isChecked)
    {
        Translation = translation;
    }
}