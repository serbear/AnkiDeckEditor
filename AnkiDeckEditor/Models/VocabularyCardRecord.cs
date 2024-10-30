using System.Collections.Generic;

namespace AnkiDeckEditor.Models;

public abstract class VocabularyCardRecord : IVocabularyCardRecord
{
    // Check box for the data grid control on the Card List tab.
    public bool IsChecked { get; set; }
    public SpeechPartToggleItem? SpeechPart { get; set; }
    public string VocabularyEntryText { get; set; }

    public string LiteralTranslationText { get; set; }
    public string LiteraryTranslationText { get; set; }
    public string OriginalText { get; set; }


    public List<int> LiteralTranslationSelection { get; set; }
    public List<int> LiteraryTranslationSelection { get; set; }
    public List<int> OriginalTextSelection { get; set; }
}