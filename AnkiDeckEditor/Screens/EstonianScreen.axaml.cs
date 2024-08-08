using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;

namespace AnkiDeckEditor.Screens;

public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        // DeckConfigTabControl.SelectedIndex = 2;
        DataContext = new EstonianScreenViewModel();
    }
}