using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views;
using Splat;
using EstonianScreenViewModel = AnkiDeckEditor.ViewModels.EstonianScreen.EstonianScreenViewModel;

namespace AnkiDeckEditor;

// ReSharper disable once PartialTypeWithSinglePart
public partial class App : Application
{
    public static MainWindowViewModel MainWindow => Locator.Current.GetService<MainWindowViewModel>()!;
    public static EstonianScreenViewModel EstonianScreen => Locator.Current.GetService<EstonianScreenViewModel>()!;
    public static EnglishScreenViewModel EnglishScreen => Locator.Current.GetService<EnglishScreenViewModel>()!;


    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Register DI for view models.
        SplatRegistrations.Register<MainWindowViewModel>();
        SplatRegistrations.Register<EstonianScreenViewModel>();
        SplatRegistrations.Register<EnglishScreenViewModel>();
        SplatRegistrations.SetupIOC();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = MainWindow
            };

        base.OnFrameworkInitializationCompleted();
    }
}