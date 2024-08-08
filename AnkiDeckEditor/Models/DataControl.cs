namespace AnkiDeckEditor.Models;

/// <summary>
/// Represents an entry in the list of postpositions and prepositions in the
/// Verb Control section.
/// </summary>
public class DataControl
{
    /// <summary>
    /// Sets the selected status of the list item.
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Sets or returns the caption of a list item.
    /// </summary>
    public string? Content { get; set; }
}