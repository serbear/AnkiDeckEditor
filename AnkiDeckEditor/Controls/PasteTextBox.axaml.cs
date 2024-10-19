using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls;

// ReSharper disable once PartialTypeWithSinglePart
public partial class PasteTextBox : UserControl
{
    private string _text;

    public static readonly DirectProperty<PasteTextBox, string> TextProperty =
        AvaloniaProperty.RegisterDirect<PasteTextBox, string>("Text", o => o.Text, (o, v) => o.Text = v);

    private string _title;

    public static readonly DirectProperty<PasteTextBox, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<PasteTextBox, string>("Title", o => o.Title, (o, v) => o.Title = v);

    public PasteTextBox()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public string Text
    {
        get => _text;
        set => SetAndRaise(TextProperty, ref _text, value);
    }

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
}