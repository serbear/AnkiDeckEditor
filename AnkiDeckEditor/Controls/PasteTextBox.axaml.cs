using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls;

public partial class PasteTextBox : UserControl
{
    public PasteTextBox()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}