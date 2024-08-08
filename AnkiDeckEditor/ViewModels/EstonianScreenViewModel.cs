using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public class EstonianScreenViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> CopyButtonCommand { get; }


    public ReactiveCommand<Button, Unit> CopyFieldClipboardCommand { get; }

    private string? _title;
    private bool _isChecked;

    public ObservableCollection<EstonianScreenViewModel> ToDoItems { get; } =
        new();

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }


    public EstonianScreenViewModel()
    {
        CopyButtonCommand = ReactiveCommand.Create(CopyCommandExecute);
        CopyFieldClipboardCommand = ReactiveCommand.Create<Button>(
            CopyDeckFieldClipboardExecute);

        ToDoItems.Add(new EstonianScreenViewModel("Value 0", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 1", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 2", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 3", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 4", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 5", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 6", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 7", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 8", false));
        ToDoItems.Add(new EstonianScreenViewModel("Value 9", false));
    }

    public EstonianScreenViewModel(string title, bool isChecked)
    {
        Title = title;
        IsChecked = isChecked;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void CopyCommandExecute()
    {
        if (Application.Current?.ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void CopyDeckFieldClipboardExecute(Button sender)
    {
        // WordByWordTranslationAnkiField 
        // LiteraryTranslationAnkiField 
        // OriginalAnkiField 
        // SpeechPartAnkiField 
        // WordFormsAnkiField 
        // VerbControlAnkiField 
        // MainEntityAnkiField 


        Console.WriteLine("do");
    }
}