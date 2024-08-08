namespace AnkiDeckEditor.Models;

public class Crockery
{
    public string Title { get; set; }
    public bool IsChecked { get; set; }

    public Crockery(string title, bool isChecked)
    {
        IsChecked = isChecked;
        Title = title;
    }
}