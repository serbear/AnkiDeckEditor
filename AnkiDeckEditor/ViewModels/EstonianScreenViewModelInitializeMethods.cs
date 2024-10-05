using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.Services.FieldsCopy;
using Avalonia.Controls;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public partial class EstonianScreenViewModel
{
    private void InitializeSubscriptions()
    {
        this.WhenAnyValue(x => x.VocabularyEntryText).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.NominativeCaseSingularWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.GenitiveCaseSingularWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.PartitiveCaseSingularWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.NominativeCasePluralWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.GenitiveCasePluralWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.PartitiveCasePluralWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.ShortIllativeCaseWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.MaInfinitiveWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.DaInfinitiveWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.IndicativeMoodWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.PassiveParticiplePastTenseWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.ThirdPersonPastTenseWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.ActiveParticipleWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.ImperativeMoodSingularWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.PassiveVoicePresentTenseWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.LiteralTranslationText).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.LiteraryTranslationText).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue(x => x.OriginalText).Subscribe(OnFieldTextChanged);
    }

    private void InitializeCommands()
    {
        CopyFieldClipboardCommand = ReactiveCommand.Create<StrategyNames>(CopyDeckFieldClipboardExecute);
        PasteCommand = ReactiveCommand.Create<Control>(PasteCommandExecute);
        SelectDeckCommand = ReactiveCommand.Create(SelectDeckExecute);
        NewEntityCommand = ReactiveCommand.Create(NewEntityExecute);
        AddListCommand = ReactiveCommand.Create(AddListExecute);
        ClearFormCommand = ReactiveCommand.Create(ClearFormExecute);
        ExportFileCommand = ReactiveCommand.Create(ExportFileExecute);
        ExitCommand = ReactiveCommand.Create(ExitExecute);
        RemoveListCommand = ReactiveCommand.Create(RemoveListCommandExecute);
    }

    private void InitializeCollections()
    {
        VerbControlItems = CollectionLoader.LoadVerbControls();
        SpeechPartItems = CollectionLoader.LoadSpeechParts();

        WordByWordContextSelectedItems = [];
        LiteraryContextSelectedItems = [];
        OriginalContextSelectedItems = [];

        EntityContextCollections = new Dictionary<string, ObservableCollection<ContextToggleItem>>
        {
            { "WordForWordTextBox", WordByWordContextSelectedItems },
            { "LiteraryTextBox", LiteraryContextSelectedItems },
            { "OriginalTextBox", OriginalContextSelectedItems }
        };

        foreach (StrategyNames name in Enum.GetValues(typeof(StrategyNames)))
            // Key - avalonia control tag.
            // Value - tuple (the copy strategy class full name, data to copy, FieldTags string)
            CopyStrategyDict.Add(name, name.StrategyFullName());

        CopyStrategyDataDict = new Dictionary<StrategyNames, object>
        {
            { StrategyNames.LiteralTranslation, WordByWordContextSelectedItems },
            { StrategyNames.LiteraryTranslation, LiteraryContextSelectedItems },
            { StrategyNames.OriginalText, OriginalContextSelectedItems },
            { StrategyNames.SpeechPart, SpeechPartItems },
            { StrategyNames.SpeechPartGovernment, VerbControlItems },
            { StrategyNames.NonVerbWordForms, (Func<NonVerbWordFormCollection>)GetNonVerbWordForms },
            { StrategyNames.VerbWordForms, (Func<VerbWordFormCollection>)GetVerbWordForms }
        };
    }

    private void InitializeFlags()
    {
        IsVerbFormsTabItemVisible = false;
        IsWordFormsTabItemVisible = false;
        IsAddEntityButtonEnabled = false;
    }
}