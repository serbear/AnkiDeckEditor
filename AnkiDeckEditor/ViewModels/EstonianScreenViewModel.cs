using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace AnkiDeckEditor.ViewModels;

public partial class EstonianScreenViewModel : ViewModelBase
{
    public EstonianScreenViewModel()
    {
        // commands
        CopyFieldClipboardCommand = ReactiveCommand.Create<StrategyNames>(CopyDeckFieldClipboardExecute);
        PasteCommand = ReactiveCommand.Create<Control>(PasteCommandExecute);
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
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
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

    private static async void PasteCommandExecute(Control control)
    {
        if (control is not TextBox textBox) throw new Exception("The control must be type of TextBox.");
        textBox.Text = await Clipboard.Get();
    }
}