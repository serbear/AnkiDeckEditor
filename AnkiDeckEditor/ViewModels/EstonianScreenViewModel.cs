using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using AnkiDeckEditor.Libs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public struct FieldTags()
{
    public const string TranslationOriginalTemplate =
        "<div class=\"sentence\">{1}</div>";

    public const string SelectedEntityTemplate = "<span>{1}</span>";

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public const string SpeechPartTemplate = "{1} <span>▪️</span> {2}";

    /// <summary>
    /// 1 - items
    /// </summary>
    public const string SimpleWordTemplate =
        "<div class=\"word-forms-container\">{1}</div>";

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

    public const string VerbTemplate =
        "<div class=\"word-forms-container\">{1}</div>";

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

    public const string VerbControlTemplate =
        "<div class=\"verb-control-container\">{1}</div>";

    public const string VerbControlItemTemplate = "<span>{1}</span>";
}

public class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }
    public ReactiveCommand<Control, Unit> CopyFieldClipboardCommand { get; }

    public ReactiveCommand<object, Unit> CopyWordFormsFieldClipboardCommand
    {
        get;
    }

    public ReactiveCommand<object, Unit> CopyVerbFormsDeckFieldClipboardCommand
    {
        get;
    }

    public ObservableCollection<VerbControlViewModel> VerbControlItems { get; }

    private ObservableCollection<ItemViewModel> _speechPartItems;

    public ObservableCollection<ItemViewModel> SpeechPartItems
    {
        get => _speechPartItems;
        set => this.RaiseAndSetIfChanged(ref _speechPartItems, value);
    }

    private ObservableCollection<ContextSelectedViewModel>
        _wordByWordContextSelectedItems;

    public ObservableCollection<ContextSelectedViewModel>
        WordByWordContextSelectedItems
    {
        get => _wordByWordContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _wordByWordContextSelectedItems,
            value);
    }

    private ObservableCollection<ContextSelectedViewModel>
        _literaryContextSelectedItems;

    public ObservableCollection<ContextSelectedViewModel>
        LiteraryContextSelectedItems
    {
        get => _literaryContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _literaryContextSelectedItems,
            value);
    }

    private ObservableCollection<ContextSelectedViewModel>
        _originalContextSelectedItems;

    public ObservableCollection<ContextSelectedViewModel>
        OriginalContextSelectedItems
    {
        get => _originalContextSelectedItems;
        set => this.RaiseAndSetIfChanged(ref _originalContextSelectedItems,
            value);
    }


    public EstonianScreenViewModel()
    {
        // commands
        CopyButtonCommand = ReactiveCommand.Create(CopyCommandExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<Control>(
            CopyDeckFieldClipboardExecute);
        CopyWordFormsFieldClipboardCommand =
            ReactiveCommand.Create<object>(
                CopyWordFormsDeckFieldClipboardExecute);
        CopyVerbFormsDeckFieldClipboardCommand =
            ReactiveCommand.Create<object>(
                CopyVerbFormsDeckFieldClipboardExecute);


        // collections
        VerbControlItems =
        [
            new VerbControlViewModel("Value 0", false),
            new VerbControlViewModel("Value 1", false),
            new VerbControlViewModel("Value 2", false),
            new VerbControlViewModel("Value 3", false),
            new VerbControlViewModel("Value 4", false),
            new VerbControlViewModel("Value 5", false),
            new VerbControlViewModel("Value 6", false),
            new VerbControlViewModel("Value 7", false),
            new VerbControlViewModel("Value 8", false),
            new VerbControlViewModel("Value 9", false)
        ];
        SpeechPartItems =
        [
            new ItemViewModel("существительное", "nimisõna", false),
            new ItemViewModel("наречие", "määrsõna", false),
            new ItemViewModel("прилагательное", "omadussõna", false),
            new ItemViewModel("местоимение", "asesõna", false),
            new ItemViewModel("глагол", "tegusõna", false)
        ];
        WordByWordContextSelectedItems = [];
        LiteraryContextSelectedItems = [];
        OriginalContextSelectedItems = [];
    }


    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void CopyCommandExecute()
    {
        if (Application.Current?.ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void CopyWordFormsDeckFieldClipboardExecute(object values)
    {
        var wordForms = FieldTags.SimpleWordItemsTemplate;
        var fieldIndex = 0;

        // Get only word forms. Skip last element.
        var formCollection =
            ((IEnumerable<object>)values).Reverse().Skip(1).Reverse();

        foreach (string wordForm in formCollection)
        {
            fieldIndex++;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // ReSharper disable once MergeConditionalExpression
            var replaceSource = wordForm == null ? "&mdash;" : wordForm;
            wordForms = wordForms.Replace($"{{{fieldIndex}}}", replaceSource);
        }

        var result = FieldTags.SimpleWordTemplate.Replace("{1}", wordForms);
        Clipboard.Get().SetTextAsync(result);
    }

    private void CopyVerbFormsDeckFieldClipboardExecute(object values)
    {
        var wordForms = FieldTags.VerbItemsTemplate;
        var fieldIndex = 0;

        // Get only word forms. Skip last element.
        var formCollection =
            ((IEnumerable<object>)values).Reverse().Skip(1).Reverse();

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
        if (sender.Tag == null)
            throw new Exception("The TextBox has no a tag.");

        var result = "";

        // WordByWordTranslationAnkiField 
        if (sender.Tag.Equals("WordByWordTranslationAnkiField"))
        {
            var resultBuilder = new StringBuilder();

            foreach (var item in WordByWordContextSelectedItems)
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace(
                        "{1}",
                        item.Title);
                    resultBuilder.Append($"{tagged} ");
                }
                else
                {
                    resultBuilder.Append($"{item.Title} ");
                }


            result = FieldTags.TranslationOriginalTemplate.Replace(
                "{1}",
                resultBuilder.ToString().Trim());
        }

        // LiteraryTranslationAnkiField 
        if (sender.Tag.Equals("LiteraryTranslationAnkiField"))
        {
            var resultBuilder = new StringBuilder();

            foreach (var item in LiteraryContextSelectedItems)
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace(
                        "{1}",
                        item.Title);
                    resultBuilder.Append($"{tagged} ");
                }
                else
                {
                    resultBuilder.Append($"{item.Title} ");
                }


            result = FieldTags.TranslationOriginalTemplate.Replace(
                "{1}",
                resultBuilder.ToString().Trim());
        }

        // OriginalAnkiField 
        if (sender.Tag.Equals("OriginalAnkiField"))
        {
            var resultBuilder = new StringBuilder();

            foreach (var item in OriginalContextSelectedItems)
                if (item.IsChecked)
                {
                    var tagged = FieldTags.SelectedEntityTemplate.Replace(
                        "{1}",
                        item.Title);
                    resultBuilder.Append($"{tagged} ");
                }
                else
                {
                    resultBuilder.Append($"{item.Title} ");
                }


            result = FieldTags.TranslationOriginalTemplate.Replace(
                "{1}",
                resultBuilder.ToString().Trim());
        }

        // SpeechPartAnkiField 
        if (sender.Tag.Equals("SpeechPartAnkiField"))
        {
            var filtered = SpeechPartItems.First(Filter);

            result = FieldTags.SpeechPartTemplate
                .Replace("{1}", filtered.Title)
                .Replace("{2}", filtered.Translation);
        }

        // MainEntityAnkiField
        if (sender.Tag.Equals("MainEntityAnkiField"))
            result = ((TextBox)sender).Text;

        // VerbControlAnkiField
        if (sender.Tag.Equals("VerbControlAnkiField"))
        {
            var selectedVerbControls = VerbControlItems.Where(e => e.IsChecked);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var verbControl in selectedVerbControls)
            {
                var item =
                    FieldTags.VerbControlItemTemplate
                        .Replace("{1}", verbControl.Title);
                result += item;
            }

            result = FieldTags.VerbControlTemplate.Replace("{1}", result);
        }

        Clipboard.Get().SetTextAsync(result);
    }


    // ReSharper disable once MemberCanBeMadeStatic.Local
    private bool Filter(ItemViewModel checkBoxItemView)
    {
        return checkBoxItemView.IsChecked.Equals(true);
    }
}