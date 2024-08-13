using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Views.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        DataContext = new EstonianScreenViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// 
    /// </summary>
    private void SpeechPartCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;
        UncheckAllCheckBoxes(ref speechPartItems);
        SwitchTabItems(ref dataContext, ref sender);
    }

    private static void UncheckAllCheckBoxes(ref ObservableCollection<SpeechPartToggleItem> toggleItems)
    {
        // Uncheck all items in the "Part of Speech" list.
        foreach (var check in toggleItems) check.IsChecked = false;
    }

    private static void SwitchTabItems(ref EstonianScreenViewModel dataContext, ref object? sender)
    {
        // Switch tabs between "word forms" and "verb forms" according to a selected speech part.
        var checkbox = (CheckBox)sender!;
        var isSenderIsVerbCheckbox = checkbox.Tag!.Equals(true);
        var isCheckBoxChecked = checkbox.IsChecked.Equals(true);
        var isVerbSelected = isSenderIsVerbCheckbox && isCheckBoxChecked;
        dataContext.IsWordFormsTabItemVisible = !isVerbSelected;
        dataContext.IsVerbFormsTabItemVisible = isVerbSelected;
    }

    private void WordForWordTextBox_OnTextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        var splitted = textBox.Text?.Trim().Split(" ").ToList();

        // ----- Word [word] word.

        List<string?> output = [];

        foreach (var s in splitted)
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

        UpdateEntityContextCollection(
            ((EstonianScreenViewModel)DataContext!)
            .EntityContextCollections[textBox.Name],
            output);
    }

    private static List<string> ProcessSplittedWord(string? splittedWord)
    {
        // Base case. There is no an input word.
        if (string.IsNullOrEmpty(splittedWord)) return [];

        // Base case. There is no any punctuations marks in a word.
        if (char.IsPunctuation(splittedWord.First()) == false &&
            char.IsPunctuation(splittedWord.Last()) == false)
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
        List<string?> contextWords)
        where T : ObservableCollection<ContextToggleItem>
    {
        if (contextWords == null) return;

        collection.Clear();

        foreach (var s in contextWords)
        {
            var isPunctuation = s!.Length.Equals(1) && char.IsPunctuation(s[0]);
            collection.Add(new ContextToggleItem(s, isPunctuation, false));
        }
    }
}