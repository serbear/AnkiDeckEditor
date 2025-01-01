using System.Collections;
using System.Linq;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Libs;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.ViewModels.EstonianScreen;
using Avalonia;
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
        SizeChanged += OnWindowSizeChanged;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        // todo: refact: duplicate code.
        var offset = Common.GetGoldenGoldenRatioOffset(e.NewSize.Height);
        this.FindControl<StackPanel>("VerbWordFormsStackPanel")!.Margin = new Thickness(0, 0, 0, offset);
    }

    private void ToggleButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
    {
        var dataContext = (EstonianScreenViewModel)DataContext!;
        dataContext.OnCheckboxChanged(sender as CheckBox, StrategyNames.SpeechPartGovernment);
    }

    private void SpeechGovernmentFilter_OnLetterChanged(object? sender, LetterChangedEventArgs e)
    {
        var items = this.FindControl<ItemsRepeater>("SpeechPartGovernmentFilterItems")?.ItemsSource;

        if (items == null) return;

        // Показать все, если нет выделенных букв.
        if (e.Letters.Length == 0)
        {
            ShowAllSpeechPartGovernment(items);
            return;
        }

        FilterSpeechPartGovernmentList(items, e.Letters);
    }

    private static void ShowAllSpeechPartGovernment(IEnumerable items)
    {
        foreach (var government in items) (government as ToggleItem)!.IsVisible = true;
    }

    private static void FilterSpeechPartGovernmentList(IEnumerable items, char[] letters)
    {
        foreach (var government in items)
        {
            var item = government as ToggleItem;
            var firstLetter = item!.Title![0];

            item.IsVisible = letters.Contains(firstLetter);
        }
    }
}