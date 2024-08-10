using System;
using System.Diagnostics;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnkiDeckEditor.Screens;

public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        // DeckConfigTabControl.SelectedIndex = 2;
        DataContext = new EstonianScreenViewModel();
    }

    private void ToggleButton_OnIsCheckedChanged(
        object? sender,
        RoutedEventArgs e)
    {
        // Uncheck all items in the "Part of Speech" list.
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;
        foreach (var check in speechPartItems) check.IsChecked = false;
    }

    private void WordForWordTextBox_OnTextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        var splitted = ((TextBox)sender!).Text?.Trim().Split(" ");
        if (splitted == null) return;

        if (((TextBox)sender).Name.Equals("WordForWordTextBox"))
        {
            ((EstonianScreenViewModel)DataContext!)
                .WordByWordContextSelectedItems.Clear();
            foreach (var s in splitted)
                ((EstonianScreenViewModel)DataContext!)
                    .WordByWordContextSelectedItems
                    .Add(new ContextSelectedViewModel(s, false));
        }

        if (((TextBox)sender).Name.Equals("LiteraryTextBox"))
        {
            ((EstonianScreenViewModel)DataContext!).LiteraryContextSelectedItems
                .Clear();
            foreach (var s in splitted)
                ((EstonianScreenViewModel)DataContext!)
                    .LiteraryContextSelectedItems
                    .Add(new ContextSelectedViewModel(s, false));
        }

        if (((TextBox)sender).Name.Equals("OriginalTextBox"))
        {
            ((EstonianScreenViewModel)DataContext!).OriginalContextSelectedItems
                .Clear();
            foreach (var s in splitted)
                ((EstonianScreenViewModel)DataContext!)
                    .OriginalContextSelectedItems
                    .Add(new ContextSelectedViewModel(s, false));
        }
    }
}