using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;

namespace AnkiDeckEditor.Screens;

public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        // CrockeryItemsRepeater.DataContext = new CrockeryViewModel();
        DeckConfigTabControl.SelectedIndex = 2;
        DataContext = new EstonianScreenViewModel();
    }
}