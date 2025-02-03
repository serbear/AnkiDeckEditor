using System.Collections.ObjectModel;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.ViewModels.EstonianScreen;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Common = AnkiDeckEditor.Libs.Common;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

// ReSharper disable once PartialTypeWithSinglePart
public partial class VocabularyEntryTab : UserControl
{
    public VocabularyEntryTab()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        SizeChanged += OnWindowSizeChanged;
    }

    private void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var offset = Common.GetGoldenGoldenRatioOffset(e.NewSize.Height);
        this.FindControl<StackPanel>("MainStackPanel")!.Margin = new Thickness(0, 0, 0, offset);
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        // Set reference to the TextBox of the PasteTextBox control.
        const string PASTE_TEXT_BOX_NAME = "VocabularyEntryText";
        const string TEXT_BOX_NAME = "MainTextBox";
        const string FIELD_NAME = "FirstFocusControl";
        var control = this
            .FindControl<PasteTextBox>(PASTE_TEXT_BOX_NAME)!
            .FindControl<TextBox>(TEXT_BOX_NAME);
        ControlHelper.SetControlReference((EstonianScreenViewModel)DataContext!, FIELD_NAME, control);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SpeechPartCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;

        UncheckAllCheckBoxes(ref speechPartItems);
        SwitchTabItems(ref dataContext, ref sender);
    }

    private static void UncheckAllCheckBoxes(ref ObservableCollection<SpeechPartToggleItem>? toggleItems)
    {
        // Uncheck all items in the "Part of Speech" list.
        foreach (var check in toggleItems) check.IsChecked = false;
    }

    /// <summary>
    /// Switches the word form editing tabs depending on the selected part of speech type.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="sender"></param>
    private void SwitchTabItems(ref EstonianScreenViewModel dataContext, ref object? sender)
    {
        // Switch tabs between "word forms" and "verb forms" according to a selected speech part.
        var checkbox = (CheckBox)sender;

        var isVerbSelected = checkbox.Tag != null;
        isVerbSelected &= (bool)checkbox.IsChecked!;

        dataContext.IsWordFormsTabItemVisible = !isVerbSelected & (bool)checkbox.IsChecked;
        dataContext.IsVerbFormsTabItemVisible = isVerbSelected & (bool)checkbox.IsChecked;

        dataContext.OnCheckboxChanged(sender as CheckBox, StrategyNames.SpeechPart);

        CopyInitialWordForm(sender as CheckBox, dataContext);
    }

    private static void CopyInitialWordForm(CheckBox? sender, EstonianScreenViewModel dataContext)
    {
        // Copy the name of the vocabulary entry into the "Nimetav" or "Ma-infinitiv" word form field.
        // The name of the vocabulary entry is the same as one of these forms.

        // todo: Если выбран вид части речи.

        if (!(bool)sender!.IsChecked!) return;

        if ((sender.DataContext as SpeechPartToggleItem)!.VerbType != null)
            dataContext.MaInfinitiveWordForm = dataContext.VocabularyEntryText;
        else
            dataContext.NominativeCaseSingularWordForm = dataContext.VocabularyEntryText;
    }
}