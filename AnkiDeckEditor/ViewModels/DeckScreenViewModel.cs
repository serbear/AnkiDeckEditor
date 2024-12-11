
using System;

namespace AnkiDeckEditor.ViewModels;

public abstract class DeckScreenViewModel : ViewModelBase
{
    public bool IsCollectionExported = true;

    [Obsolete("Obsolete")]
    public abstract bool ExportDeck();
}