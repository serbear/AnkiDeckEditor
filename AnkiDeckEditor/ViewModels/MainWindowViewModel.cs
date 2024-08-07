using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using Avalonia;
using ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public ReactiveCommand<Unit, Unit> MyCommand { get; }
#pragma warning restore CA1822 // Mark members as static

    public MainWindowViewModel()
    {
        MyCommand = ReactiveCommand.Create(MyCommandExecute);
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    private void MyCommandExecute()
    {
        Debug.WriteLine("The submit command was run.");
        if (Application.Current?.ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }
}