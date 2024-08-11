using ReactiveUI;

namespace AnkiDeckEditor.Models;

public class ToggleItem : ReactiveObject
{
    private string? _title;
    private bool _isChecked;

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

    public ToggleItem(string? title, bool isChecked)
    {
        Title = title;
        IsChecked = isChecked;
    }
}