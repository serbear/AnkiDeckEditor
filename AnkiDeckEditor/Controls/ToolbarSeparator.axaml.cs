using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls;

public partial class ToolbarSeparator : UserControl
{
    public ToolbarSeparator()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}