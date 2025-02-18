using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services;
using AnkiDeckEditor.ViewModels.EstonianScreen;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Common = AnkiDeckEditor.Libs.Common;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ContextTab : UserControl
{
    public ContextTab()
    {
        InitializeComponent();
        SizeChanged += OnWindowSizeChanged;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var offset = Common.GetGoldenGoldenRatioOffset(e.NewSize.Height);
        this.FindControl<StackPanel>("ContextStackPanel")!.Margin = new Thickness(0, 0, 0, offset);
    }

    protected override void OnLoaded(RoutedEventArgs routedEventArgs)
    {
        var pasteTextBoxes = FieldHelper.GetChildrenList<PasteTextBox>(this);

        foreach (var innerTextBox in pasteTextBoxes
                     .Select(p => p.FindControl<TextBox>(PublicConst.PasteTextBoxMainTextBoxName)))
            innerTextBox?.GetObservable(TextBox.TextProperty).Subscribe(
                _ => WordForWordTextBox_OnTextChanged(innerTextBox, null));
    }

    private void WordForWordTextBox_OnTextChanged(object? sender, TextChangedEventArgs? e)
    {
        if (sender is not TextBox textBox) return;

        List<string?>? split = textBox.Text?.Trim().Split(" ").ToList();

        if (split == null) return;

        // ----- Word [word] word.

        List<string?>? output = [];

        foreach (var s in split)
        {
            var fullList = ProcessSplittedWord(s);

            // This is a clean word without punctuation marks.
            if (fullList.Count.Equals(1) && fullList[0].Equals(s))
            {
                output.Add(s);
                continue;
            }

            // Insert a word into its index.

            var leftPunctuationNumber = 0;

            foreach (var t in s)
                if (char.IsPunctuation(t))
                    leftPunctuationNumber++;
                else
                    break;

            // There are only punctuation marks in the string.
            if (leftPunctuationNumber == fullList.Count)
            {
                fullList.Reverse();
            }
            else
            {
                var cleanWord = fullList[0];
                fullList.RemoveAt(0);
                fullList.Reverse();
                fullList.Insert(leftPunctuationNumber, cleanWord);
            }

            output.AddRange(fullList);
        }

        // -----

        var parentContainerName = textBox.GetVisualAncestors().First(visual => visual is PasteTextBox).Name;
        var collection = ((EstonianScreenViewModel)DataContext!).EntityContextCollections[parentContainerName!];
        UpdateEntityContextCollection(collection, output);
    }

    private static List<string> ProcessSplittedWord(string? splittedWord)
    {
        // Base case. There is no an input word.
        if (string.IsNullOrEmpty(splittedWord)) return [];

        // Base case. There are no any punctuation marks in a word.
        if ((char.IsPunctuation(splittedWord.First()) == false) &
            (char.IsPunctuation(splittedWord.Last()) == false))
            return [splittedWord];

        string? addstr = null;
        if (char.IsPunctuation(splittedWord.First()))
            addstr = splittedWord.Substring(1, splittedWord.Length - 1);
        else if (char.IsPunctuation(splittedWord.Last()))
            // ReSharper disable once ReplaceSubstringWithRangeIndexer
            addstr = splittedWord.Substring(0, splittedWord.Length - 1);

        var result = ProcessSplittedWord(addstr);

        if (char.IsPunctuation(splittedWord.First()))
            addstr = splittedWord.First().ToString();
        else if (char.IsPunctuation(splittedWord.Last()))
            addstr = splittedWord.Last().ToString();

        result.Add(addstr);

        return result;

        // Return a list with one item: an original value.
    }

    private static void UpdateEntityContextCollection<T>(
        T collection,
        List<string?>? contextWords)
        where T : ObservableCollection<ContextToggleItem>
    {
        if (contextWords == null || contextWords.Count == 0) return;

        // todo: comment

        // save checked status

        var checkedWords = new Dictionary<string, int>();

        if (collection.Count > 0)
            for (var i = collection.Count - 1; i >= 0; i--)
                if (collection[i].IsChecked)
                    checkedWords.Add(collection[i].Title!, i);

        collection.Clear();

        foreach (var s in contextWords)
        {
            var isPunctuation = s!.Length.Equals(1) & char.IsPunctuation(s[0]);

            // ---
            bool isChecked;
            int savedWordIndex;
            var isSavedWord = checkedWords.TryGetValue(s, out savedWordIndex);

            // Если индекс слова и значение слова совпадают, установить выделение.
            List<bool> condition =
            [
                isSavedWord,
                contextWords.IndexOf(s).Equals(savedWordIndex)
            ];

            isChecked = condition.All(e => e.Equals(true));

            // ---
            collection.Add(new ContextToggleItem(s, isPunctuation, isChecked));
        }
    }
}