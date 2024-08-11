using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using Avalonia;
using ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;

namespace AnkiDeckEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> ShutdownCommand { get; }

    public MainWindowViewModel()
    {
        ShutdownCommand = ReactiveCommand.Create(ShutdownApplicationExecute);
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    private void ShutdownApplicationExecute()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }
}