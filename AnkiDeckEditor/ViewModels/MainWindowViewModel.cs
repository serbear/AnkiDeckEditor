using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

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
        ((ClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!).Shutdown();
    }
}