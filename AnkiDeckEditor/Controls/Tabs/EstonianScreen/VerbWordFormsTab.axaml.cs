using AnkiDeckEditor.Enums;
using AnkiDeckEditor.ViewModels.EstonianScreen;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AnkiDeckEditor.Controls.Tabs.EstonianScreen;

// ReSharper disable once PartialTypeWithSinglePart
public partial class VerbWordFormsTab : UserControl
{
    public VerbWordFormsTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ToggleButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
    {
        var dataContext = (EstonianScreenViewModel)DataContext!;
        dataContext.OnCheckboxChanged(sender as CheckBox, StrategyNames.SpeechPartGovernment);
    }
}