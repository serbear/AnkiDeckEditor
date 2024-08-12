using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Screens;

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