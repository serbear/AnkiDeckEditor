namespace AnkiDeckEditor.ViewModels;

public class ItemViewModel : AbstractItemViewModel
{
    public string? Translation { get; set; }

    public ItemViewModel(string title, string? translation, bool isChecked) :
        base(title, isChecked)
    {
        Translation = translation;
    }
}