using ReactiveUI;

namespace AnkiDeckEditor.Models;

/// <summary>
/// Represents a checkbox control.
/// </summary>
public class ToggleItem : ReactiveObject
{
    private string? _title;
    private bool _isChecked;
    private bool _isVisible;


    /// <summary>
    /// The text label of the checkbox control.
    /// </summary>
    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    /// <summary>
    /// The state indicator of the checkbox control.
    /// </summary>
    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public ToggleItem(string? title, bool isChecked)
    {
        Title = title;
        IsChecked = isChecked;
        IsVisible = true;
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }
}