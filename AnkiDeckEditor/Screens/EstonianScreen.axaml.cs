using System.ComponentModel;
using AnkiDeckEditor.ViewModels;
using Avalonia;
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

    private string _name;


    private void ToggleButton_OnIsCheckedChanged(
        object? sender,
        RoutedEventArgs e)
    {
        // Uncheck all items in the "Part of Speech" list.
        var dataContext = (EstonianScreenViewModel)DataContext!;
        var speechPartItems = dataContext.SpeechPartItems;
        foreach (var check in speechPartItems) check.IsChecked = false;
    }
}