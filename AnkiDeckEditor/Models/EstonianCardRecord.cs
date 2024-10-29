using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Models;

// todo: refact: ExpandoObject

public class EstonianCardRecord : VocabularyCardRecord
{
    // public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }

    public List<string?> SpeechPartGovernment { get; set; }
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