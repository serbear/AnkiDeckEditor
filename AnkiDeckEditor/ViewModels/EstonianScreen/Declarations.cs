using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AnkiDeckEditor.ViewModels.EstonianScreen;

public partial class EstonianScreenViewModel
{
    // Main Entity Tab

    [Reactive] public string VocabularyEntryText { get; set; }

    // Context Tab

    [Reactive] public string LiteralTranslationText { get; set; }
    [Reactive] public string LiteraryTranslationText { get; set; }
    [Reactive] public string OriginalText { get; set; }

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

    public ReactiveCommand<StrategyNames, Unit> CopyFieldClipboardCommand { get; set; }
    public ReactiveCommand<Unit, Unit> SelectDeckCommand { get; set; }
    public ReactiveCommand<Unit, Unit> NewEntityCommand { get; set; }
    public ReactiveCommand<Unit, Unit> AddListCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ExportFileCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; set; }
    public ReactiveCommand<Control, Unit> PasteCommand { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveListCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ClearCardCollectionCommand { get; set; }
    public ReactiveCommand<Unit, Unit> SaveListCommand { get; set; }

    // Flags

    [Reactive] public bool IsVerbFormsTabItemVisible { get; set; }
    [Reactive] public bool IsWordFormsTabItemVisible { get; set; }
    [Reactive] public bool IsAddEntityButtonEnabled { get; set; }
    [Reactive] public bool IsRemoveCardButtonEnabled { get; set; }
    [Reactive] public bool IsClearCardCollectionButtonEnabled { get; set; }
    [Reactive] public bool IsVisibleAddEntityListButton { get; set; }
    [Reactive] public bool IsSaveEntityListButtonEnabled { get; set; }


    // Collections

    [Reactive] public ObservableCollection<EstonianCardRecord> CardCollectionItems { get; set; } = [];
    [Reactive] public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; set; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }
    private Dictionary<StrategyNames, string?> CopyStrategyDict { get; } = new();
    private Dictionary<StrategyNames, object> CopyStrategyDataDict { get; set; }
}