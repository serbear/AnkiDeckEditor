using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Controls;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.Services.Estonian;
using AnkiDeckEditor.Services.FieldsCopy;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.ViewModels.EstonianScreen;

public partial class EstonianScreenViewModel : ViewModelBase
{
    private const string CardCollectionDataGridName = "CardCollectionDataGrid";
    private const string EstonianDeckMainTabControlName = "DeckConfigTabControl";

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    private Control RootControl { get; set; } = new();

    /// <summary>
    /// The field stores a reference to the text box that will be focused on after the form cleanup function
    /// is executed.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private Control FirstFocusControl { get; set; }

    private EditModes _currentOperationalMode;
    private EstonianCardRecord? _currentEditCard;

    public EstonianScreenViewModel()
    {
        InitializeCommands();
        InitializeCollections();
        InitializeFlags();
        InitializeSubscriptions();

        // Set default work mode of deck editor.
        _currentOperationalMode = EditModes.Add;
    }


    private void OnFieldTextChanged(string newText)
    {
        if (_currentOperationalMode == EditModes.Add) AddModeTextChanged();
        if (_currentOperationalMode == EditModes.Edit) EditModeTextChanged();
    }


    private void AddModeTextChanged()
    {
        // Set enable status for the button "Add to the Card List".
        // If all fields are empty, enable status is 'false'.
        var isTextBoxesEmpty = FieldHelper.IsUnsavedContent<PasteTextBox>(RootControl);
        IsAddEntityButtonEnabled = isTextBoxesEmpty;
    }

    private void EditModeTextChanged()
    {
        if (_currentEditCard == null) return;

        // Checks each PasteTextBox element to see if the text of its main text box matches the stored value
        // of the corresponding vocabulary card field.
        IsSaveEntityListButtonEnabled = FieldHelper.IsFieldsChanged<PasteTextBox>(RootControl, _currentEditCard);
    }


    private void RemoveListCommandExecute()
    {
        var cardCollectionItems = CardCollectionItems;

        if (CardCollection.AreManyCardsChecked(ref cardCollectionItems))
        {
            CardCollection.RemoveManyItems(ref cardCollectionItems);
        }
        else
        {
            var dataGrid = FieldHelper.GetChildren<DataGrid>(RootControl, CardCollectionDataGridName) as DataGrid;
            CardCollection.RemoveOneItem(dataGrid, ref cardCollectionItems);
        }

        UpdateIsRemoveCardButtonEnabledFlag();
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

        // Set the speech part and the speech part government checkbox.
        newCard.SpeechPart = SpeechPartItems
            .FirstOrDefault(sp => sp.IsChecked.Equals(true))?.Title!;
        newCard.SpeechPartGovernment = VerbControlItems
            .Where(e => e.IsChecked.Equals(true))
            .Select(e => e.Title).ToList();

        // Assign indexes of selected words in the context fields. 
        newCard.LiteralTranslationSelection = FieldHelper.GetContextSelectedWordIndexes(WordByWordContextSelectedItems);
        newCard.LiteraryTranslationSelection = FieldHelper.GetContextSelectedWordIndexes(LiteraryContextSelectedItems);
        newCard.OriginalTextSelection = FieldHelper.GetContextSelectedWordIndexes(OriginalContextSelectedItems);

        CardCollectionItems.Add(newCard);

        // Sort the card list alphabetically.
        CardCollectionItems = new ObservableCollection<EstonianCardRecord>(
            CardCollectionItems.OrderBy<EstonianCardRecord, string>(e => e.VocabularyEntryText));

        UpdateIsRemoveCardButtonEnabledFlag();
        NewEntityExecute();
    }

    private void NewEntityExecute()
    {
        // todo: Unsaved content.
        // ...

        FieldHelper.ClearTextFields<PasteTextBox>(RootControl);
        FieldHelper.ResetCheckBoxFields(RootControl);

        // Reset context word selections.

        WordByWordContextSelectedItems.Clear();


        // Switch to the Vocabulary Entry tab.
        RootControl.FindControl<TabControl>(EstonianDeckMainTabControlName)!.SelectedIndex =
            (int)EstonianDeckTabs.VocabularyEntryTab;
        FirstFocusControl.Focus();
    }

    internal void UpdateIsRemoveCardButtonEnabledFlag()
    {
        var dataGrid = FieldHelper.GetChildren<DataGrid>(RootControl, CardCollectionDataGridName);
        List<bool> removeCardCondition =
        [
            // The card collection contains the elements.
            (dataGrid as DataGrid)?.SelectedItems.Count > 0,
            // The tab with the list of cards is active.
            RootControl.FindControl<TabControl>(EstonianDeckMainTabControlName)!.SelectedIndex.Equals(
                (int)EstonianDeckTabs.CardCollectionTab)
        ];
        IsRemoveCardButtonEnabled = removeCardCondition.All(e => e.Equals(true));
        IsClearCardCollectionButtonEnabled = removeCardCondition[1];
    }

    public void EditCardListEntry(EstonianCardRecord? cardListEntry)
    {
        // todo: unsaved data
        //...

        _currentOperationalMode = EditModes.Edit;

        NewEntityExecute();

        // Update reactive properties.
        var estonianScreenViewModel = this;
        PropertySetter.SetReactive(ref cardListEntry, ref estonianScreenViewModel);

        // Restore checkboxes.
        var parentTabItem = RootControl.FindControl<TabControl>(EstonianDeckMainTabControlName)?
            .Items[(int)EstonianDeckTabs.VocabularyEntryTab] as Control;
        FieldHelper.RestoreCheckBoxes(parentTabItem, [cardListEntry?.SpeechPart!]);

        parentTabItem = RootControl.FindControl<TabControl>(EstonianDeckMainTabControlName)?
            .Items[(int)EstonianDeckTabs.VerbWordFormsTab] as Control;
        FieldHelper.RestoreCheckBoxes(parentTabItem, cardListEntry?.SpeechPartGovernment!);

        // Restore selected text of the context fields.
        FieldHelper.CheckContextSelectedWords(
            cardListEntry?.LiteralTranslationSelection,
            WordByWordContextSelectedItems);
        FieldHelper.CheckContextSelectedWords(
            cardListEntry?.LiteraryTranslationSelection,
            LiteraryContextSelectedItems);
        FieldHelper.CheckContextSelectedWords(
            cardListEntry?.OriginalTextSelection,
            OriginalContextSelectedItems);

        // Show 'Save' button.
        IsVisibleAddEntityListButton = false;

        _currentEditCard = cardListEntry;
    }

    private void SaveListCommandExecute()
    {
        if (_currentEditCard == null)
            throw new InvalidOperationException("There is no a vocabulary card being edited.");

        // Remove an old vocabulary card from the card collection.
        CardCollectionItems.Remove(_currentEditCard);

        AddListExecute();
        CancelEditCommandExecute();
    }

    private void CancelEditCommandExecute()
    {
        // Hide 'Save' button and show 'Add' button.
        IsVisibleAddEntityListButton = true;

        _currentEditCard = null;
        _currentOperationalMode = EditModes.Add;

        NewEntityExecute();
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
            result = FieldHelper.GetFieldValue(this, value.ClassFieldName());
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

    private void ClearCardCollectionCommandExecute()
    {
        var cardCollectionItems = CardCollectionItems;
        CardCollection.ClearCollection(ref cardCollectionItems);
        UpdateIsRemoveCardButtonEnabledFlag();
    }
}