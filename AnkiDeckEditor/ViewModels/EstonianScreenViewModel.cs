using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.Services.FieldsCopy;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AnkiDeckEditor.ViewModels;

public partial class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }
    public ReactiveCommand<StrategyNames, Unit> CopyFieldClipboardCommand { get; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }
    [Reactive] public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }

    [Reactive] public bool IsVerbFormsTabItemVisible { get; set; }
    [Reactive] public bool IsWordFormsTabItemVisible { get; set; }
    public ReactiveCommand<Control, Unit> PasteFromClipboardCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectDeckCommand { get; }
    public ReactiveCommand<Unit, Unit> NewEntityCommand { get; }
    public ReactiveCommand<Unit, Unit> AddListCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }
    public ReactiveCommand<Unit, Unit> ExportFileCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    private Dictionary<StrategyNames, string?> CopyStrategyDict { get; } = new();
    private Dictionary<StrategyNames, object> CopyStrategyDataDict { get; }


    public EstonianScreenViewModel()
    {
        // commands
        CopyButtonCommand = ReactiveCommand.Create(ExitButtonExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<StrategyNames>(CopyDeckFieldClipboardExecute);
        PasteFromClipboardCommand = ReactiveCommand.Create<Control>(PasteFromClipboardExecute);
        SelectDeckCommand = ReactiveCommand.Create(SelectDeckExecute);
        NewEntityCommand = ReactiveCommand.Create(NewEntityExecute);
        AddListCommand = ReactiveCommand.Create(AddListExecute);
        ClearFormCommand = ReactiveCommand.Create(ClearFormExecute);
        ExportFileCommand = ReactiveCommand.Create(ExportFileExecute);
        ExitCommand = ReactiveCommand.Create(ExitExecute);

        // Collections
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

        // Toggles
        IsVerbFormsTabItemVisible = false;
        IsWordFormsTabItemVisible = false;

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

    private NonVerbWordFormCollection GetNonVerbWordForms()
    {
        return new NonVerbWordFormCollection(
            [
                NominativeCaseSingularWordForm,
                GenitiveCaseSingularWordForm,
                PartitiveCaseSingularWordForm,
                NominativeCasePluralWordForm,
                GenitiveCasePluralWordForm,
                PartitiveCasePluralWordForm,
                ShortIllativeCaseWordForm
            ]
        );
    }

    private VerbWordFormCollection GetVerbWordForms()
    {
        return new VerbWordFormCollection(
        [
            MaInfinitiveWordForm,
            DaInfinitiveWordForm,
            IndicativeMoodWordForm,
            PassiveParticiplePastTenseWordForm,
            ThirdPersonPastTenseWordForm,
            ActiveParticipleWordForm,
            ImperativeMoodSingularWordForm,
            PassiveVoicePresentTenseWordForm
        ]);
    }

    private string GetFieldValue(string fieldName)
    {
        // Get this class type.
        var type = GetType();

        // ReSharper disable once GrammarMistakeInComment
        // Expression 'f.Name[1..]' means: skip the "$" symbol in the name of the class field.
        var field = type.GetRuntimeFields().First(f => f.Name[1..].Equals(fieldName));

        if (field == null) throw new ArgumentException($"Field with the name '{fieldName}' is not found.");

        // If the field is public and exists, return its value.
        return (string)field.GetValue(this)!;
    }

    private void ExitExecute()
    {
        throw new NotImplementedException();
    }

    private void ExportFileExecute()
    {
        throw new NotImplementedException();
    }

    private void ClearFormExecute()
    {
        throw new NotImplementedException();
    }

    private void AddListExecute()
    {
        throw new NotImplementedException();
    }

    private void NewEntityExecute()
    {
        throw new NotImplementedException();
    }

    private void SelectDeckExecute()
    {
        Console.WriteLine("Select deck");
    }

    private static async void PasteFromClipboardExecute(Control value)
    {
        var text = await Clipboard.Get();
        ((TextBox)value).Text = text;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void ExitButtonExecute()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void CopyDeckFieldClipboardExecute(StrategyNames value)
    {
        var strategy = Activator.CreateInstance(Type.GetType(CopyStrategyDict[value])!);
        var copyContext = new Context();
        copyContext.SetStrategy((strategy as ICopyStrategy)!);

        // Try to get data collection for the strategy.
        // If there is no data collection, take a string value of the field with name 'value'.
        var isDataCollectionExist = CopyStrategyDataDict.TryGetValue(value, out var fieldValue);

        object result;

        if (!isDataCollectionExist)
            result = GetFieldValue(value.ClassFieldName());
        else
            result = fieldValue is Func<NonVerbWordFormCollection> func
                ? func()
                : fieldValue!;

        copyContext.DoCopyLogic(result);
    }
}