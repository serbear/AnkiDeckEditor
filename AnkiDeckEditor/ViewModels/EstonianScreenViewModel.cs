using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AnkiDeckEditor.ViewModels;

public class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }
    public ReactiveCommand<Control, Unit> CopyFieldClipboardCommand { get; }
    public ReactiveCommand<object, Unit> CopyWordFormsFieldClipboardCommand { get; }
    public ReactiveCommand<object, Unit> CopyVerbFormsDeckFieldClipboardCommand { get; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }
    [Reactive] public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }
    [Reactive] public bool IsVerbFormsTabItemVisible { get; set; }
    [Reactive] public bool IsWordFormsTabItemVisible { get; set; }

    public EstonianScreenViewModel()
    {
        // commands
        CopyButtonCommand = ReactiveCommand.Create(ExitButtonExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<Control>(CopyDeckFieldClipboardExecute);
        CopyWordFormsFieldClipboardCommand = ReactiveCommand.Create<object>(CopyWordFormsDeckFieldClipboardExecute);
        CopyVerbFormsDeckFieldClipboardCommand = ReactiveCommand.Create<object>(CopyVerbFormsDeckFieldClipboardExecute);

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
    }


    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void ExitButtonExecute()
    {
        
        
        
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private static void CopyWordFormsDeckFieldClipboardExecute(object values)
    {
        var wordForms = FieldTags.SimpleWordItemsTemplate;
        var fieldIndex = 0;

        // Get only word forms. Skip the last element (a reference to a parent element of a button).
        var formCollection = ((IEnumerable<object>)values).Reverse().Skip(1).Reverse();

        foreach (string wordForm in formCollection)
        {
            fieldIndex++;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // ReSharper disable once MergeConditionalExpression
            // If a word form is absent, insert long dash instead.
            var replaceSource = wordForm == null ? "&mdash;" : wordForm;
            wordForms = wordForms.Replace($"{{{fieldIndex}}}", replaceSource);
        }

        var result = FieldTags.SimpleWordTemplate.Replace("{1}", wordForms);
        Clipboard.Get().SetTextAsync(result);
    }

    private static void CopyVerbFormsDeckFieldClipboardExecute(object values)
    {
        var wordForms = FieldTags.VerbItemsTemplate;
        var fieldIndex = 0;

        // Get only word forms. Skip last element.
        var formCollection = ((IEnumerable<object>)values).Reverse().Skip(1).Reverse();

        foreach (string wordForm in formCollection)
        {
            fieldIndex++;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // ReSharper disable once MergeConditionalExpression
            var replaceSource = wordForm == null ? "&mdash;" : wordForm;
            wordForms = wordForms.Replace($"{{{fieldIndex}}}", replaceSource);
        }

        var result = FieldTags.VerbTemplate.Replace("{1}", wordForms);
        Clipboard.Get().SetTextAsync(result);
    }

    private void CopyDeckFieldClipboardExecute(Control sender)
    {
        if (sender.Tag == null) throw new Exception("The TextBox has no a tag.");

        var result = "";

        // WordByWordTranslationAnkiField 
        if (sender.Tag.Equals("WordByWordTranslationAnkiField"))
        {
            var resultBuilder = new List<string>();

            foreach (var item in WordByWordContextSelectedItems)
                // Marked to learn entity.
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace("{1}", item.Title);
                    resultBuilder.Add($"{tagged}");
                }
                // A common word or punctuation.
                else
                {
                    resultBuilder.Add($"{item.Title} ");
                }

            var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
                .AddSpaseAfterCloseHtmlTag()
                .RemoveLeftSpaceFromPunctuation()
                .AddSpaceAfterClosePunctuation()
                .RemoveRightSpaceClosePunctuation()
                .RemoveLeftSpaceClosePunctuation();

            result = FieldTags.TranslationOriginalTemplate.Replace("{1}", sm.ResultString);
        }

        // LiteraryTranslationAnkiField 
        if (sender.Tag.Equals("LiteraryTranslationAnkiField"))
        {
            var resultBuilder = new List<string>();

            foreach (var item in LiteraryContextSelectedItems)
                // Marked to learn entity.
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace("{1}", item.Title);
                    resultBuilder.Add($"{tagged}");
                }
                // A common word or punctuation.
                else
                {
                    resultBuilder.Add($"{item.Title} ");
                }

            var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
                .AddSpaseAfterCloseHtmlTag()
                .RemoveLeftSpaceFromPunctuation()
                .AddSpaceAfterClosePunctuation()
                .RemoveRightSpaceClosePunctuation()
                .RemoveLeftSpaceClosePunctuation();

            result = FieldTags.TranslationOriginalTemplate.Replace("{1}", sm.ResultString);
        }

        // OriginalAnkiField 
        if (sender.Tag.Equals("OriginalAnkiField"))
        {
            var resultBuilder = new List<string>();

            foreach (var item in OriginalContextSelectedItems)
                // Marked to learn entity.
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace("{1}", item.Title);
                    resultBuilder.Add($"{tagged}");
                }
                // A common word or punctuation.
                else
                {
                    resultBuilder.Add($"{item.Title} ");
                }

            var sm = new StringManipulator(string.Join("", resultBuilder).Trim())
                .AddSpaseAfterCloseHtmlTag()
                .RemoveLeftSpaceFromPunctuation()
                .AddSpaceAfterClosePunctuation()
                .RemoveRightSpaceClosePunctuation()
                .RemoveLeftSpaceClosePunctuation();

            result = FieldTags.TranslationOriginalTemplate.Replace("{1}", sm.ResultString);
        }

        // SpeechPartAnkiField 
        if (sender.Tag.Equals("SpeechPartAnkiField"))
        {
            var filtered = SpeechPartItems.First(e => e.IsChecked);

            result = FieldTags.SpeechPartTemplate
                .Replace("{1}", filtered.Title)
                .Replace("{2}", filtered.Translation);
        }

        // MainEntityAnkiField
        if (sender.Tag.Equals("MainEntityAnkiField")) result = ((TextBox)sender).Text;

        // VerbControlAnkiField
        if (sender.Tag.Equals("VerbControlAnkiField"))
        {
            var selectedVerbControls = VerbControlItems.Where(e => e.IsChecked);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var verbControl in selectedVerbControls)
            {
                var item = FieldTags.VerbControlItemTemplate.Replace("{1}", verbControl.Title);
                result += item;
            }

            result = FieldTags.VerbControlTemplate.Replace("{1}", result);
        }

        Clipboard.Get().SetTextAsync(result);
    }
}