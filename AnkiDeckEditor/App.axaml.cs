using System;
using System.Collections.Generic;
using System.IO;
using AnkiDeckEditor.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Logging;
using Splat;

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

        // ReSharper disable once UnusedParameter.Local
        var loggerFactory = LoggerFactory.Create(builder => builder.AddFilter(logLevel => true).AddDebug());

        var build = Locator.CurrentMutable;
        build.RegisterLazySingleton(() => (IDialogService)new DialogService(
            new DialogManager(
                new ViewLocator(),
                logger: loggerFactory.CreateLogger<DialogManager>(),
                dialogFactory: new DialogFactory().AddFluent()),
            x => Locator.Current.GetService(x)));

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
                DataContext = new MainWindowViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }

}