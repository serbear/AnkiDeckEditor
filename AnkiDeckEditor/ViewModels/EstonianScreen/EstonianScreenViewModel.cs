using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
using AnkiDeckEditor.Views.Dialogs;
using DialogHostAvalonia;
using Common = AnkiDeckEditor.Libs.Common;

namespace AnkiDeckEditor.ViewModels.EstonianScreen;

// Changing the IsCollectionExported flag in the current version of the application:
// Controlling changes in the number of word cards in the collection.

public partial class EstonianScreenViewModel : DeckScreenViewModel
{
    private const string CARD_COLLECTION_DATA_GRID_NAME = "CardCollectionDataGrid";
    private const string ESTONIAN_DECK_MAIN_TAB_CONTROL_NAME = "DeckConfigTabControl";
    private const string COLLECTION_COUNTER_NAME = "CollectionCounter";
    private const string COLLECTION_COUNTER_TEXT_BLOCK_NAME = "CollectionCounterValue";

    private EstonianCardRecord? _currentEditCard;
    private EditModes _currentOperationalMode;
    private bool _saveResult;

    public EstonianScreenViewModel()
    {
        InitializeCommands();
        InitializeCollections();
        InitializeFlags();
        InitializeSubscriptions();

        // Set default work mode of deck editor.
        _currentOperationalMode = EditModes.Add;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    private Control RootControl { get; set; } = new();

    /// <summary>
    /// The field stores a reference to the text box that will be focused on after the form cleanup function
    /// is executed.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private Control FirstFocusControl { get; set; }

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
            var dataGrid = FieldHelper.GetChildren<DataGrid>(RootControl, CARD_COLLECTION_DATA_GRID_NAME) as DataGrid;
            CardCollection.RemoveOneItem(dataGrid, ref cardCollectionItems);
        }

        UpdateCollectionCounter();
        UpdateIsRemoveCardButtonEnabledFlag();
        UpdateExportButtonEnableFlag();

        IsVisibleAddEntityListButton = true;
        IsSaveEntityListButtonEnabled = false;

        IsCollectionExported = CardCollectionItems.Count == 0;

        _currentOperationalMode = EditModes.Add;
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

    [Obsolete("Obsolete")]
    private async Task ExitExecute()
    {
        if (IsCollectionExported) return;

        _ = await Services.Common.ExportDeckOnExit(this);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    [Obsolete("Obsolete")]
    private async void ExportFileExecute()
    {
        await ExportDeck();

        // Show the export result message on successful export.
        if (_saveResult)
            _ = (bool)(await DialogHost.Show(new ExportResultDialog(), PublicConst.MainDialogHost))!;
        else
            Console.WriteLine("Cannot save.");
    }

    [Obsolete("Obsolete")]
    public override async Task<bool> ExportDeck()
    {
        // todo: Various export errors. Show messages.
        // todo: Move strings to resources.

        const string FILE_DIALOG_TITLE = "Сохранить коллекцию";
        const string FILE_TYPE_NAME = "CSV Text Files";
        const string FILE_TYPE_EXTENSION = "csv";

        var saveFileDialog = new SaveFileDialog
        {
            Title = FILE_DIALOG_TITLE,
            Filters = [new FileDialogFilter { Name = FILE_TYPE_NAME, Extensions = { FILE_TYPE_EXTENSION } }],
            DefaultExtension = FILE_TYPE_EXTENSION
        };

        var filePath = await saveFileDialog.ShowAsync(PublicConst.MainWindowReference!);

        if (!string.IsNullOrEmpty(filePath))
            _saveResult = DeckExporter.ExportCollection(CardCollectionItems, CopyStrategyDict, filePath);

        // todo: no file name actions.

        IsCollectionExported = _saveResult;
        return _saveResult;
    }

    private static void ClearFormExecute()
    {
        // todo: implement
        throw new NotImplementedException();
    }

    private void AddListExecute()
    {
        var newCard = new EstonianCardRecord();
        var estonianScreenViewModel = this;

        PropertySetter.Set(ref estonianScreenViewModel, ref newCard);

        // Set the speech part and the speech part government checkbox.
        newCard.SpeechPart = SpeechPartItems!.FirstOrDefault(sp => sp.IsChecked.Equals(true));
        // newCard.SpeechPart = SpeechPartItems.FirstOrDefault(sp => sp.IsChecked.Equals(true))?.Title!;

        newCard.SpeechPartGovernment = VerbControlItems!
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

        UpdateCollectionCounter();
        UpdateExportButtonEnableFlag();
        UpdateIsRemoveCardButtonEnabledFlag();
        NewEntityExecute();
    }

    private void UpdateExportButtonEnableFlag()
    {
        IsExportButtonEnabled = CardCollectionItems.Count > 0;
    }

    private void UpdateCollectionCounter()
    {
        var counter = FieldHelper.GetChildrenList<Border>(RootControl)
            .FirstOrDefault(e => e.Name == COLLECTION_COUNTER_NAME);
        if (counter == null) throw new InvalidOperationException("The collection counter control not found.");

        // Show the counter if the collection is not empty.
        counter.IsVisible = CardCollectionItems.Count > 0;

        var counterTextBlock = FieldHelper.GetChildrenList<TextBlock>(RootControl)
            .FirstOrDefault(e => e.Name == COLLECTION_COUNTER_TEXT_BLOCK_NAME);
        if (counterTextBlock == null)
            throw new InvalidOperationException("The collection counter text block not found.");

        counterTextBlock.Text = CardCollectionItems.Count.ToString();
        IsCollectionEmpty = CardCollectionItems.Count > 0;
    }

    private void NewEntityExecute()
    {
        // todo: Unsaved content.
        // ...

        _currentOperationalMode = EditModes.Add;
        IsCollectionExported = false;
        CancelEditCommandExecute();
    }

    private void ClearAndSwitchVocabularyEntry()
    {
        ClearControlData();
        // Switch to the Vocabulary Entry tab.
        RootControl.FindControl<TabControl>(ESTONIAN_DECK_MAIN_TAB_CONTROL_NAME)!.SelectedIndex =
            (int)EstonianDeckTabs.VocabularyEntryTab;
        FirstFocusControl.Focus();
    }

    private void ClearControlData()
    {
        FieldHelper.ClearTextFields<PasteTextBox>(RootControl);
        FieldHelper.ResetCheckBoxFields(RootControl);
        ClearContextSelectedItems();
    }

    private void ClearContextSelectedItems()
    {
        WordByWordContextSelectedItems?.Clear();
        LiteraryContextSelectedItems?.Clear();
        OriginalContextSelectedItems?.Clear();
    }

    internal void UpdateIsRemoveCardButtonEnabledFlag()
    {
        var dataGrid = FieldHelper.GetChildren<DataGrid>(RootControl, CARD_COLLECTION_DATA_GRID_NAME);
        var isListTabActive = RootControl.FindControl<TabControl>(ESTONIAN_DECK_MAIN_TAB_CONTROL_NAME)!
            .SelectedIndex.Equals((int)EstonianDeckTabs.CardCollectionTab);

        // Conditions
        List<bool> removeCardCondition =
        [
            // The card collection contains selected elements.
            (dataGrid as DataGrid)?.SelectedItems.Count > 0,
            // The tab with the list of cards is active.
            isListTabActive
        ];
        List<bool> clearDeckCondition =
        [
            // The tab with the list of cards is active.
            isListTabActive,
            // There is at least one item in the list.
            CardCollectionItems.Count > 0
        ];

        IsRemoveCardButtonEnabled = Common.IsAllValuesAreTrue(removeCardCondition);
        IsClearCardCollectionButtonEnabled = Common.IsAllValuesAreTrue(clearDeckCondition);
    }

    public void EditCardListEntry(EstonianCardRecord? cardListEntry)
    {
        // todo: unsaved data
        //...

        _currentOperationalMode = EditModes.Edit;

        ClearAndSwitchVocabularyEntry();

        // Update reactive properties.
        var estonianScreenViewModel = this;
        PropertySetter.SetReactive(ref cardListEntry, ref estonianScreenViewModel);

        // Restore checkboxes.
        var parentTabItem = RootControl.FindControl<TabControl>(ESTONIAN_DECK_MAIN_TAB_CONTROL_NAME)?
            .Items[(int)EstonianDeckTabs.VocabularyEntryTab] as Control;
        FieldHelper.RestoreCheckBoxes(parentTabItem, cardListEntry?.SpeechPart);

        parentTabItem = RootControl.FindControl<TabControl>(ESTONIAN_DECK_MAIN_TAB_CONTROL_NAME)?
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

        ClearAndSwitchVocabularyEntry();
    }

    private static void SelectDeckExecute()
    {
        // todo: implement
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

        copyContext.DoCopyLogic(result, out _);
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
        UpdateCollectionCounter();
        UpdateExportButtonEnableFlag();
        IsCollectionExported = true;
    }
}