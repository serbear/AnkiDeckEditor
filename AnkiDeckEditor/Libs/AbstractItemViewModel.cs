using ReactiveUI;

namespace AnkiDeckEditor.ViewModels;

public abstract class AbstractItemViewModel : ViewModelBase
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

    public AbstractItemViewModel(string title, bool isChecked)
    {
        Title = title;
        IsChecked = isChecked;
    }
}