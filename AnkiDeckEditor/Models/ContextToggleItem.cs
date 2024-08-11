namespace AnkiDeckEditor.Models;

public class ContextToggleItem : ToggleItem
{
    public bool IsPunctuation { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ContextToggleItem(string? title, bool isPunctuation, bool isChecked)
        : base(title, isChecked)
    {
        IsPunctuation = isPunctuation;
    }
}