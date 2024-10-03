using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls;

public partial class AdeToolbar : UserControl
{
    public AdeToolbar()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}