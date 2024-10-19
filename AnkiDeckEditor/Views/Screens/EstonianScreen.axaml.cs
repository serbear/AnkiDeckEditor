using AnkiDeckEditor.Services;
using AnkiDeckEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EstonianScreenViewModel = AnkiDeckEditor.ViewModels.EstonianScreen.EstonianScreenViewModel;

namespace AnkiDeckEditor.Views.Screens;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EstonianScreen : UserControl
{
    public EstonianScreen()
    {
        InitializeComponent();
        DataContext = new EstonianScreenViewModel();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        // Set reference to this UserControl.
        const string FIELD_NAME = "RootControl";
        ControlHelper.SetControlReference((EstonianScreenViewModel)DataContext!, FIELD_NAME, this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void DeckConfigTabControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        (DataContext as EstonianScreenViewModel)?.UpdateIsRemoveCardButtonEnabledFlag();
    }
}