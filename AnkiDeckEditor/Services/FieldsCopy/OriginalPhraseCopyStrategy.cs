using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Implement functionality for copying the Original Phrase field data into the clipboard.
/// </summary>
public class OriginalPhraseCopyStrategy : ICopyStrategy
{
    public string DoCopy<T>(ObservableCollection<T> data)
    {
        var result = Common.ProcessCollection(data);
        return result;
    }
}