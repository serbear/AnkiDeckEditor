using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Views.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EnglishScreen : UserControl
{
    public EnglishScreen()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}