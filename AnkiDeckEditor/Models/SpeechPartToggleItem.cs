namespace AnkiDeckEditor.Models;

public class SpeechPartToggleItem : ToggleItem
{
    public string? Translation { get; }
    public bool IsVerb { get; }

    public SpeechPartToggleItem(string? title, string? translation, bool isChecked) : base(title, isChecked)
    {
        Translation = translation;
        IsVerb = false;
    }

    public SpeechPartToggleItem(string? title, string? translation, bool isVerb, bool isChecked)
        : base(title, isChecked)
    {
        Translation = translation;
        IsVerb = isVerb;
    }
}