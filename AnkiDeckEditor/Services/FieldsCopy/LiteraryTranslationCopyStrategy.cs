using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Implement functionality for copying the Literary Translation field data into the clipboard.
/// </summary>
public class LiteraryTranslationCopyStrategy : ICopyStrategy
{
    public string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        var result = Common.ProcessCollection(data);
        return result;
    }
}