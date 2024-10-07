using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Models;

// todo: refact: ExpandoObject

public class EstonianCardRecord
{
    // Check box for the data grid control on the Card List tab.
    public bool IsChecked { get; set; }

    public string VocabularyEntryText { get; set; }
    public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; set; }
    public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }

    public string LiteralTranslationText { get; set; }
    public string LiteraryTranslationText { get; set; }
    public string OriginalText { get; set; }

    public string NominativeCaseSingularWordForm { get; set; }
    public string GenitiveCaseSingularWordForm { get; set; }
    public string PartitiveCaseSingularWordForm { get; set; }
    public string NominativeCasePluralWordForm { get; set; }
    public string GenitiveCasePluralWordForm { get; set; }
    public string PartitiveCasePluralWordForm { get; set; }
    public string ShortIllativeCaseWordForm { get; set; }

    public string MaInfinitiveWordForm { get; set; }
    public string DaInfinitiveWordForm { get; set; }
    public string IndicativeMoodWordForm { get; set; }
    public string PassiveParticiplePastTenseWordForm { get; set; }
    public string ThirdPersonPastTenseWordForm { get; set; }
    public string ActiveParticipleWordForm { get; set; }
    public string ImperativeMoodSingularWordForm { get; set; }
    public string PassiveVoicePresentTenseWordForm { get; set; }
}