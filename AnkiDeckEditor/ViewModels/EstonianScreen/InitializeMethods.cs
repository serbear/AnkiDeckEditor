using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.Services.FieldsCopy;
using Avalonia.Controls;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels.EstonianScreen;

public partial class EstonianScreenViewModel
{
    private void InitializeSubscriptions()
    {
        // todo: refact: automatically subscribe

        // Text boxes
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.VocabularyEntryText).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.NominativeCaseSingularWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.GenitiveCaseSingularWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.PartitiveCaseSingularWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.NominativeCasePluralWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.GenitiveCasePluralWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.PartitiveCasePluralWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.ShortIllativeCaseWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.MaInfinitiveWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.DaInfinitiveWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.IndicativeMoodWordForm).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.PassiveParticiplePastTenseWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.ThirdPersonPastTenseWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.ActiveParticipleWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.ImperativeMoodSingularWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.PassiveVoicePresentTenseWordForm)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.LiteralTranslationText).Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.LiteraryTranslationText)
            .Subscribe(OnFieldTextChanged);
        this.WhenAnyValue<EstonianScreenViewModel, string>(x => x.OriginalText).Subscribe(OnFieldTextChanged);
    }


    [Obsolete("Obsolete")]
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
        ClearCardCollectionCommand = ReactiveCommand.Create(ClearCardCollectionCommandExecute);
        SaveListCommand = ReactiveCommand.Create(SaveListCommandExecute);
        CancelEditCommand = ReactiveCommand.Create(CancelEditCommandExecute);
    }

    private void InitializeCollections()
    {
        LoadSpeechPartGovernmentList();
        PublicConst.EstonianDeckTemplates = CollectionLoader.LoadFieldTags();

        SpeechPartItems = CollectionLoader.LoadSpeechParts();

        WordByWordContextSelectedItems = [];
        LiteraryContextSelectedItems = [];
        OriginalContextSelectedItems = [];

        EntityContextCollections = new Dictionary<string, ObservableCollection<ContextToggleItem>?>
        {
            { "LiteralTranslationText", WordByWordContextSelectedItems },
            { "LiteraryTranslationText", LiteraryContextSelectedItems },
            { "OriginalText", OriginalContextSelectedItems }
        };

        foreach (StrategyNames name in Enum.GetValues(typeof(StrategyNames)))
            // Key - avalonia control tag.
            // Value - tuple (the copy strategy class full name, data to copy, FieldTags string)
            CopyStrategyDict.Add(name, name.StrategyFullName());

        CopyStrategyDataDict = new Dictionary<StrategyNames, object?>
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

    private void LoadSpeechPartGovernmentList()
    {
        VerbControlItems = CollectionLoader.LoadVerbControls();
        if (VerbControlItems != null)
            SpeechGovernmentFilterLetters = GetSpeechPartGovernmentFilterLetters();
    }


    private char[] GetSpeechPartGovernmentFilterLetters()
    {
        return VerbControlItems
            .Aggregate(new HashSet<char>(), (current, verbControlItem) =>
            {
                current.Add(char.ToUpper(verbControlItem.Title![0]));
                return current;
            }).ToArray();
    }

    private void InitializeFlags()
    {
        IsVerbFormsTabItemVisible = false;
        IsWordFormsTabItemVisible = false;
        IsAddEntityButtonEnabled = false;
        IsRemoveCardButtonEnabled = false;
        IsClearCardCollectionButtonEnabled = false;
        IsVisibleAddEntityListButton = true;
        IsSaveEntityListButtonEnabled = false;
        IsExportButtonEnabled = false;
    }
}