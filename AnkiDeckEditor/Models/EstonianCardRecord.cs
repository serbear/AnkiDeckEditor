using System.Collections.Generic;

namespace AnkiDeckEditor.Models;

// todo: refact: ExpandoObject

public class EstonianCardRecord : VocabularyCardRecord
{
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