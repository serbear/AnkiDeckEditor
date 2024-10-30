namespace AnkiDeckEditor;

public struct PublicConst
{
    /// <summary>
    /// The identifier of the top-level dialog screen.
    /// </summary>
    public const string MainDialogHost = "MainDialogHost";

    /// <summary>
    /// The marker identified the absence of a word form.
    /// </summary>
    public const string LongDashHtmlCode = "&mdash;";

    public const string PasteTextBoxMainTextBoxName = "MainTextBox";
}

/// <summary>
/// Types of verb.
/// </summary>
public enum VerbTypes
{
    /// <summary>
    /// Simple verb: consists of a single word.
    /// </summary>
    Simple,

    /// <summary>
    /// Compound verb: consists of more than one word.
    /// </summary>
    Compound
}