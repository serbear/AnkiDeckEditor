using System.Collections.Generic;
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
    string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        throw new System.NotImplementedException();
    }

    string DoCopyString(string data)
    {
        throw new System.NotImplementedException();
    }

    string DoCopyList(List<string> data)
    {
        throw new System.NotImplementedException();
    }

    string DoCopyWordForms(WordFormsCollectionBase data)
    {
        throw new System.NotImplementedException();
    }

    // string DoCopyList(List<object> data)
    // {
    //     throw new System.NotImplementedException();
    // }
}