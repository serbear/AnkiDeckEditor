using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Views.Dialogs;

public partial class ExitDialog : UserControl
{
    public ExitDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}