using System;
using System.Threading.Tasks;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views.Dialogs;
using DialogHostAvalonia;

namespace AnkiDeckEditor.Services;

public static class Common
{
    [Obsolete("Obsolete")]
    public static async Task<bool> ExportDeckOnExit(DeckScreenViewModel? deck)
    {
        var result = false;

        // Do not ask confirmation if the deck is empty.
        if (!deck!.IsCollectionEmpty) return result;

        var dialogResult = (bool)(await DialogHost.Show(new ExportCollectionDialog(), PublicConst.MainDialogHost))!;

        if (dialogResult) result = await deck.ExportDeck();

        return result;
    }
}