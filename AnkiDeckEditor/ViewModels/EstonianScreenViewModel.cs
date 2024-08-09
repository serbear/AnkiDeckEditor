using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using AnkiDeckEditor.Libs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public struct FieldTags()
{
    public const string TranslationOriginal =
        "<div class=\"sentence\">{1}</div>";

    public string SelectedEntity = "<span>{1}</span>";

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public const string SpeechPart = "{1} <span>▪️</span> {2}";

    /// <summary>
    /// 1 - items
    /// </summary>
    public const string SimpleWord =
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
    public const string SimpleWordItems =
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

    public const string Verb = "<div class=\"word-forms-container\">{1}</div>";

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
    public const string VerbItems =
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

    public string VerbControl =
        "<div class=\"verb-control-container\">{1}</div>";

    public string VerbControlItem = "<span>{1}</span>";
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


    private FieldTags _fieldTags = new();


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
        var wordForms = FieldTags.SimpleWordItems;
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

        var result = FieldTags.SimpleWord.Replace("{1}", wordForms);
        Clipboard.Get().SetTextAsync(result);
    }

    private void CopyVerbFormsDeckFieldClipboardExecute(object values)
    {
        var wordForms = FieldTags.VerbItems;
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

        var result = FieldTags.Verb.Replace("{1}", wordForms);
        Clipboard.Get().SetTextAsync(result);
    }

    private void CopyDeckFieldClipboardExecute(Control sender)
    {
        if (sender.Tag == null)
            throw new Exception("The TextBox has no a tag.");

        var result = "";

        // WordByWordTranslationAnkiField 
        if (sender.Tag.Equals("WordByWordTranslationAnkiField"))
            // todo: select <span>
            result = FieldTags.TranslationOriginal.Replace(
                "{1}",
                ((TextBox)sender).Text);
        // LiteraryTranslationAnkiField 
        if (sender.Tag.Equals("LiteraryTranslationAnkiField"))
            result = FieldTags.TranslationOriginal.Replace(
                "{1}",
                ((TextBox)sender).Text);
        // OriginalAnkiField 
        if (sender.Tag.Equals("OriginalAnkiField"))
            result = FieldTags.TranslationOriginal.Replace(
                "{1}",
                ((TextBox)sender).Text);
        // SpeechPartAnkiField 
        if (sender.Tag.Equals("SpeechPartAnkiField"))
        {
            var filtered = SpeechPartItems.First(Filter);

            result = FieldTags.SpeechPart
                .Replace("{1}", filtered.Title)
                .Replace("{2}", filtered.Translation);
        }

        if (sender.Tag.Equals("MainEntityAnkiField"))
            result = ((TextBox)sender).Text;
        Clipboard.Get().SetTextAsync(result);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private bool Filter(ItemViewModel checkBoxItemView)
    {
        return checkBoxItemView.IsChecked.Equals(true);
    }
}