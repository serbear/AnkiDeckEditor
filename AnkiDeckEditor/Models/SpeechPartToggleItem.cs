namespace AnkiDeckEditor.Models;

public class SpeechPartToggleItem : ToggleItem
{
    public string? Translation { get; }
    public bool IsVerb { get; }
    public VerbTypes? VerbType { get; set; }

    public SpeechPartToggleItem(string? title, string? translation, bool isChecked) : base(title, isChecked)
    {
        Translation = translation;
        IsVerb = false;
        VerbType = null;
    }


    public SpeechPartToggleItem(string? title, string? translation, bool isVerb, bool isChecked)
        : base(title, isChecked)
    {
        Translation = translation;
        IsVerb = isVerb;
    }

    public SpeechPartToggleItem(string? title, string? translation, VerbTypes verbType, bool isChecked)
        : base(title, isChecked)
    {
        Translation = translation;
        VerbType = verbType;
    }
}