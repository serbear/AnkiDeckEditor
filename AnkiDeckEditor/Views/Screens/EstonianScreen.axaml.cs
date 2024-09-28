using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
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
}