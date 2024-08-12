using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public struct FieldTags
{
    public const string TranslationOriginalTemplate = "<div class=\"sentence\">{1}</div>";

    public const string SelectedEntityTemplate = "<span>{1}</span>";

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public const string SpeechPartTemplate = "{1} <span>▪️</span> {2}";

    /// <summary>
    /// 1 - items
    /// </summary>
    public const string SimpleWordTemplate = "<div class=\"word-forms-container\">{1}</div>";

    /// <summary>
    /// 1 - ains nim<br/>
    /// 2 - ains om<br/>
    /// 3 - ains os<br/>
    /// 4 - mitm nim<br/>
    /// 5 - mitm om<br/>
    /// 6 - mitm os<br/>
    /// 7 - sse <br/>
    /// </summary>
    public const string SimpleWordItemsTemplate =
        "<div class=\"grid-item amount\">AIN.</div>\n" +
        "<div class=\"grid-item form\">{1}</div>\n" +
        "<div class=\"grid-item form\">{2}</div>\n" +
        "<div class=\"grid-item form\">{3}</div>\n" +
        "<div class=\"grid-item amount\">MIT.</div>\n" +
        "<div class=\"grid-item form\">{4}</div>\n" +
        "<div class=\"grid-item form\">{5}</div>\n" +
        "<div class=\"grid-item form\">{6}</div>\n" +
        "<div class=\"grid-item short-into\">L.SSE.</div>\n" +
        "<div class=\"grid-item form-short-into\">{7}</div>";

    public const string VerbTemplate = "<div class=\"word-forms-container\">{1}</div>";

    /// <summary>
    /// 1 - ma<br/>
    /// 2 - 3p.min<br/>
    /// 3 - da<br/>
    /// 4 - nud<br/>
    /// 5 - 3p<br/>
    /// 6 - 2p.kas<br/>
    /// 7 - tud<br/>
    /// 8 - akse<br/>
    /// </summary>
    public const string VerbItemsTemplate =
        "<div class=\"grid-item declension\">MA</div>\n" +
        "<div class=\"grid-item form\">{1}</div>\n" +
        "<div class=\"grid-item declension\">3P.MIN</div>\n" +
        "<div class=\"grid-item form\">{5}</div>\n" +
        "<div class=\"grid-item declension\">DA</div>\n" +
        "<div class=\"grid-item form\">{2}</div>\n" +
        "<div class=\"grid-item declension\">NUD</div>\n" +
        "<div class=\"grid-item form\">{6}</div>\n" +
        "<div class=\"grid-item declension\">3P</div>\n" +
        "<div class=\"grid-item form\">{3}</div>\n" +
        "<div class=\"grid-item declension\">2P.KÄS</div>\n" +
        "<div class=\"grid-item form\">{7}</div>\n" +
        "<div class=\"grid-item declension\">TUD</div>\n" +
        "<div class=\"grid-item form\">{4}</div>\n" +
        "<div class=\"grid-item declension\">AKSE</div>\n" +
        "<div class=\"grid-item form\">{8}</div>";

    public const string VerbControlTemplate = "<div class=\"verb-control-container\">{1}</div>";

    public const string VerbControlItemTemplate = "<span>{1}</span>";
}

public class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }
    public ReactiveCommand<Control, Unit> CopyFieldClipboardCommand { get; }

    public ReactiveCommand<object, Unit> CopyWordFormsFieldClipboardCommand { get; }

    public ReactiveCommand<object, Unit> CopyVerbFormsDeckFieldClipboardCommand { get; }

    public ObservableCollection<ToggleItem> VerbControlItems { get; }

    private ObservableCollection<SpeechPartToggleItem> _speechPartItems;

    public ObservableCollection<SpeechPartToggleItem> SpeechPartItems
    {
        get => _speechPartItems;
        set => this.RaiseAndSetIfChanged(ref _speechPartItems, value);
    }

    private ObservableCollection<ContextToggleItem> _wordByWordContextSelectedItems;

    public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems
    {
        get => _wordByWordContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _wordByWordContextSelectedItems, value);
    }

    private ObservableCollection<ContextToggleItem> _literaryContextSelectedItems;

    public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems
    {
        get => _literaryContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _literaryContextSelectedItems, value);
    }

    private ObservableCollection<ContextToggleItem> _originalContextSelectedItems;

    public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems
    {
        get => _originalContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _originalContextSelectedItems, value);
    }

    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }
    private bool _isVerbFormsTabItemVisible;

    public bool IsVerbFormsTabItemVisible
    {
        get => _isVerbFormsTabItemVisible;
        set => this.RaiseAndSetIfChanged(ref _isVerbFormsTabItemVisible, value);
    }

    private bool _isWordFormsTabItemVisible;

    public bool IsWordFormsTabItemVisible
    {
        get => _isWordFormsTabItemVisible;
        set => this.RaiseAndSetIfChanged(ref _isWordFormsTabItemVisible, value);
    }

    public EstonianScreenViewModel()
    {
        // commands
        CopyButtonCommand = ReactiveCommand.Create(CopyCommandExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<Control>(CopyDeckFieldClipboardExecute);
        CopyWordFormsFieldClipboardCommand = ReactiveCommand.Create<object>(CopyWordFormsDeckFieldClipboardExecute);
        CopyVerbFormsDeckFieldClipboardCommand = ReactiveCommand.Create<object>(CopyVerbFormsDeckFieldClipboardExecute);

        // collections
        VerbControlItems =
        [
            new ToggleItem("Value 0", false),
            new ToggleItem("Value 1", false),
            new ToggleItem("Value 2", false),
            new ToggleItem("Value 3", false),
            new ToggleItem("Value 4", false),
            new ToggleItem("Value 5", false),
            new ToggleItem("Value 6", false),
            new ToggleItem("Value 7", false),
            new ToggleItem("Value 8", false),
            new ToggleItem("Value 9", false)
        ];
        SpeechPartItems =
        [
            new SpeechPartToggleItem("существительное", "nimisõna", false),
            new SpeechPartToggleItem("наречие", "määrsõna", false),
            new SpeechPartToggleItem("прилагательное", "omadussõna", false),
            new SpeechPartToggleItem("местоимение", "asesõna", false),
            new SpeechPartToggleItem("глагол", "tegusõna", true, false)
        ];

        WordByWordContextSelectedItems = [];
        LiteraryContextSelectedItems = [];
        OriginalContextSelectedItems = [];

        EntityContextCollections = new Dictionary<string, ObservableCollection<ContextToggleItem>>
        {
            { "WordForWordTextBox", WordByWordContextSelectedItems },
            { "LiteraryTextBox", LiteraryContextSelectedItems },
            { "OriginalTextBox", OriginalContextSelectedItems }
        };

        IsVerbFormsTabItemVisible = false;
        IsWordFormsTabItemVisible = false;
    }


    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void CopyCommandExecute()
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