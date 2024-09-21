using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <para>
/// The interface declares operations common to all supported methods of copying data to the clipboard.
/// </para>
/// <para>
/// Context <c>AnkiDeckEditor.Services.FieldsCopy.Context</c> uses this interface to invoke an algorithm defined by
/// specific copy strategies.
/// </para>
public interface ICopyStrategy
{
    string DoCopy<T>(ObservableCollection<T> data);
}