using System;
using System.Linq;
using System.Reflection;
using AnkiDeckEditor.Controls;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.Services.FieldsCopy;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.ViewModels;

public partial class EstonianScreenViewModel : ViewModelBase
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private Control RootControl { get; set; }

    /// <summary>
    /// The field stores a reference to the text box that will be focused on after the form cleanup function
    /// is executed.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private Control FirstFocusControl { get; set; }


    public EstonianScreenViewModel()

    {
        InitializeCommands();
        InitializeCollections();
        InitializeFlags();
        InitializeSubscriptions();
    }


    private void OnFieldTextChanged(string newText)
    {
        // Set enable status for the button "Add to the Card List".
        // If all fields are empty, enable status is 'false'.
        var isTextBoxesEmpty = FieldHelper.IsUnsavedContent<PasteTextBox>(RootControl);
        IsAddEntityButtonEnabled = isTextBoxesEmpty;
    }


    private void RemoveListCommandExecute()
    {
        throw new NotImplementedException();
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

    // todo: refact: make universal method.
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
        var newCard = new EstonianCardRecord();
        var estonianScreenViewModel = this;

        PropertySetter.Set(ref estonianScreenViewModel, ref newCard);
        CardCollectionItems.Add(newCard);

        // todo: sort list alphabetically.
        //...

        NewEntityExecute();
    }

    public void EditCardListEntry(EstonianCardRecord? cardListEntry)
    {
        // todo: unsaved data
        //...

        var estonianScreenViewModel = this;
        PropertySetter.SetReactive(ref cardListEntry, ref estonianScreenViewModel);
        // todo: обновить reactive property
    }

    private void NewEntityExecute()
    {
        FieldHelper.ClearFields<PasteTextBox>(RootControl);
        // Switch to the Vocabulary Entry tab.
        RootControl.FindControl<TabControl>("DeckConfigTabControl")!.SelectedIndex = 0;
        // Put focus on the Vocabulary Entry text box.
        FirstFocusControl.Focus();
    }

    private void SelectDeckExecute()
    {
        throw new NotImplementedException();
    }

    private void CopyDeckFieldClipboardExecute(StrategyNames value)
    {
        var strategy = Activator.CreateInstance(Type.GetType(CopyStrategyDict[value]!)!);
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

    private void SetButtonEnable()
    {
        Console.WriteLine("set button enable");
    }
}