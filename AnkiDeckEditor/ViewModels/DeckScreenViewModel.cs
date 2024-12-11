namespace AnkiDeckEditor.ViewModels;

public abstract class DeckScreenViewModel : ViewModelBase
{
    public bool IsCollectionExported = true;
    public abstract bool ExportDeck();
}