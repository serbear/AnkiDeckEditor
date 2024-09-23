namespace AnkiDeckEditor;

// ReSharper disable once IdentifierTypo
public struct PublicConsts
{
    /// <summary>
    /// The identifier of the top-level dialog screen.
    /// </summary>
    public const string MainDialogHost = "MainDialogHost";

    /// <summary>
    /// The marker identified the absence of a word form.
    /// </summary>
    public const string LongDashHtmlCode = "&mdash;";
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