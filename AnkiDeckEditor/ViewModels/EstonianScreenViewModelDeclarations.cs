using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using Avalonia.Controls;
using ReactiveUI;
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

    // Commands

    public ReactiveCommand<StrategyNames, Unit> CopyFieldClipboardCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectDeckCommand { get; }
    public ReactiveCommand<Unit, Unit> NewEntityCommand { get; }
    public ReactiveCommand<Unit, Unit> AddListCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }
    public ReactiveCommand<Unit, Unit> ExportFileCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Control, Unit> PasteCommand { get; }

    // Flags

    [Reactive] public bool IsVerbFormsTabItemVisible { get; set; }
    [Reactive] public bool IsWordFormsTabItemVisible { get; set; }

    // Collections

    [Reactive] public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; }
    private Dictionary<StrategyNames, string?> CopyStrategyDict { get; } = new();
    private Dictionary<StrategyNames, object> CopyStrategyDataDict { get; }
}