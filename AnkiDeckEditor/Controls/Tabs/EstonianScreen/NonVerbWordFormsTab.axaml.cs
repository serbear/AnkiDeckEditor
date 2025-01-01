using AnkiDeckEditor.Libs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

public partial class NonVerbWordFormsTab : UserControl
{
    public NonVerbWordFormsTab()
    {
        InitializeComponent();
        SizeChanged += OnWindowSizeChanged;
    }

    private void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        // todo: refact: duplicate code.
        var offset = Common.GetGoldenGoldenRatioOffset(e.NewSize.Height);
        this.FindControl<StackPanel>("NonVerbWordFormsStackPanel")!.Margin = new Thickness(0, 0, 0, offset);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}