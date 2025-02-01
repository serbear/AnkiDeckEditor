using System;
using System.Threading.Tasks;

namespace AnkiDeckEditor.ViewModels;

public abstract class DeckScreenViewModel : ViewModelBase
{
    public bool IsCollectionExported;

    [Obsolete("Obsolete")]
    public abstract Task<bool> ExportDeck();
}