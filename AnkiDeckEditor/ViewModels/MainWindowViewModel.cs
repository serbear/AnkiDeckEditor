using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using AnkiDeckEditor.Views.Dialogs;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DialogHostAvalonia;
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
    private async void ShutdownApplicationExecute()
    {
        // Ask user for exit confirmation.
        // var dialogResult = (bool)(await DialogHost.Show(new ExitDialog(), PublicConst.MainDialogHost))!;

        // Other values - Exit is not confirmed.
        // bool[] checks =
        // [
        // dialogResult,
        // Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
        // ];

        // Check the user's answer.
        // True, True - User has confirmed exit.
        // if (checks is not [true, true]) return;
        ((ClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!).Shutdown();
    }
}