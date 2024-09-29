using System.Collections.Generic;
using AnkiDeckEditor.Services.FieldsCopy;

namespace AnkiDeckEditor.Enums;

public enum StrategyNames
{
    LiteralTranslation,
    LiteraryTranslation,
    OriginalText,
    SpeechPart,
    VocabularyEntry,
    SpeechPartGovernment,
    NonVerbWordForms,
    VerbWordForms
}

public static class StrategyNamesExtensions
{
    private static readonly Dictionary<StrategyNames, string> Names = new()
    {
        { StrategyNames.VocabularyEntry, "VocabularyEntryText" }
    };

    private static readonly Dictionary<StrategyNames, string> StrategyFullNames = new()
    {
        { StrategyNames.LiteralTranslation, typeof(LiteralTranslationCopyStrategy).FullName! },
        { StrategyNames.LiteraryTranslation, typeof(LiteraryTranslationCopyStrategy).FullName! },
        { StrategyNames.OriginalText, typeof(OriginalPhraseCopyStrategy).FullName! },
        { StrategyNames.SpeechPart, typeof(SpeechPartCopyStrategy).FullName! },
        { StrategyNames.VocabularyEntry, typeof(VocabularyEntryCopyStrategy).FullName! },
        { StrategyNames.SpeechPartGovernment, typeof(SpeechPartGovernmentCopyStrategy).FullName! },
        { StrategyNames.NonVerbWordForms, typeof(WordFormsCopyStrategy).FullName! },
        { StrategyNames.VerbWordForms, typeof(WordFormsCopyStrategy).FullName! }
    };

    public static string ClassFieldName(this StrategyNames sn)
    {
        return Names[sn];
    }

    public static string StrategyFullName(this StrategyNames sn)
    {
        return StrategyFullNames[sn];
    }
}