using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

public class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }
    public ReactiveCommand<Control, Unit> CopyFieldClipboardCommand { get; }
    public ReactiveCommand<ReadOnlyCollection<object>, Unit> CopyWordFormsFieldClipboardCommand { get; }
    public ObservableCollection<ToggleItem> VerbControlItems { get; }
    public Dictionary<string, ObservableCollection<ContextToggleItem>> EntityContextCollections { get; set; }
    [Reactive] public ObservableCollection<SpeechPartToggleItem> SpeechPartItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> WordByWordContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> LiteraryContextSelectedItems { get; set; }
    [Reactive] public ObservableCollection<ContextToggleItem> OriginalContextSelectedItems { get; set; }
    [Reactive] public bool IsVerbFormsTabItemVisible { get; set; }
    [Reactive] public bool IsWordFormsTabItemVisible { get; set; }
    public ReactiveCommand<Control, Unit> PasteFromClipboardCommand { get; }

    private readonly Dictionary<string, (string, object)> _copyStrategyDict;

    public EstonianScreenViewModel()
    {
        // commands
        CopyButtonCommand = ReactiveCommand.Create(ExitButtonExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<Control>(CopyDeckFieldClipboardExecute);

        CopyWordFormsFieldClipboardCommand =
            ReactiveCommand.Create<ReadOnlyCollection<object>>(CopyWordFormsDeckFieldClipboardExecute);

        PasteFromClipboardCommand = ReactiveCommand.Create<Control>(PasteFromClipboardExecute);

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

        // Key - avalonia control tag.
        // Value - tuple (the copy strategy class full name, data to copy, FieldTags string)
        _copyStrategyDict = new Dictionary<string, (string, object)>
        {
            {
                "WordByWordTranslationAnkiField",
                (typeof(LiteralTranslationCopyStrategy).FullName, WordByWordContextSelectedItems)!
            },
            {
                "LiteraryTranslationAnkiField",
                (typeof(LiteraryTranslationCopyStrategy).FullName, LiteraryContextSelectedItems)!
            },
            { "OriginalAnkiField", (typeof(OriginalPhraseCopyStrategy).FullName, OriginalContextSelectedItems)! },
            { "SpeechPartAnkiField", (typeof(SpeechPartCopyStrategy).FullName, SpeechPartItems)! },
            { "MainEntityAnkiField", (typeof(MainEntityCopyStrategy).FullName, null)! },
            { "VerbControlAnkiField", (typeof(VerbGovernmentCopyStrategy).FullName, VerbControlItems)! },
            { "WordFormsAnkiField", (typeof(WordFormsCopyStrategy).FullName, null)! }
        };
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

    private static void CopyWordFormsDeckFieldClipboardExecute(ReadOnlyCollection<object> values)
    {
        var copyContext = new Context();
        copyContext.SetStrategy(new WordFormsCopyStrategy());
        copyContext.DoCopyLogic(values.ToList());
    }


    private void CopyDeckFieldClipboardExecute(Control sender)
    {
        if (sender.Tag == null) throw new Exception("The Control has no a tag.");

        var copyContext = new Context();

        var strategy = Activator.CreateInstance(Type.GetType(_copyStrategyDict[(string)sender.Tag].Item1)!);
        copyContext.SetStrategy((strategy as ICopyStrategy)!);

        object itemCollection;
        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (sender.Tag.Equals("MainEntityAnkiField"))
            itemCollection = ((TextBox)sender).Text!;
        else
            itemCollection = _copyStrategyDict[(string)sender.Tag].Item2;
        copyContext.DoCopyLogic(itemCollection);
    }
}