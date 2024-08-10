namespace AnkiDeckEditor.Models;

public class ContextSelectedItem(string title, bool isChecked)
{
    public string Title { get; set; } = title;
    public bool IsChecked { get; set; } = isChecked;
}