using ReactiveUI.Fody.Helpers;

namespace AnkiDeckEditor.ViewModels;

public partial class EstonianScreenViewModel
{
    // Main Entity Tab

    [Reactive] public string VocabularyEntryText { get; set; }

    // Word forms (verb and non-verb) tab

    [Reactive] public string NominativeCaseSingularWordForm { get; set; }
    [Reactive] public string GenitiveCaseSingularWordForm { get; set; }
    [Reactive] public string PartitiveCaseSingularWordForm { get; set; }
    [Reactive] public string NominativeCasePluralWordForm { get; set; }
    [Reactive] public string GenitiveCasePluralWordForm { get; set; }
    [Reactive] public string PartitiveCasePluralWordForm { get; set; }
    [Reactive] public string ShortIllativeCaseWordForm { get; set; }

    // Verb word forms tab

    [Reactive] public string MaInfinitiveWordForm { get; set; } // ma
    [Reactive] public string DaInfinitiveWordForm { get; set; } // da
    [Reactive] public string IndicativeMoodWordForm { get; set; } // b, mina-vorm
    [Reactive] public string PassiveParticiplePastTenseWordForm { get; set; } //  tud
    [Reactive] public string ThirdPersonPastTenseWordForm { get; set; } // s, past of tema 
    [Reactive] public string ActiveParticipleWordForm { get; set; } // nud
    [Reactive] public string ImperativeMoodSingularWordForm { get; set; } // you do
    [Reactive] public string PassiveVoicePresentTenseWordForm { get; set; } // takse, dakse
}