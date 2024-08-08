namespace AnkiDeckEditor.Models;

/// <summary>
/// Represents an entry in the list of postpositions and prepositions in the
/// Verb Control section.
/// </summary>
public class VerbControlItem(string title, bool isChecked)
{
    /// <summary>
    /// Sets or returns the caption of a list item.
    /// </summary> 
    public string Title { get; set; } = title;

    /// <summary>
    /// Sets the selected status of the list item.
    /// </summary> 
    public bool IsChecked { get; set; } = isChecked;
}