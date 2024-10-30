using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnkiDeckEditor.Models;

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
    string DoCopyCollection<T>(ObservableCollection<T> data);
    string DoCopyString(string data);
    string DoCopyList(List<string> data);
    string DoCopyValueTuple(ValueTuple<string, List<int>> data);
    string DoCopyWordForms(WordFormsCollectionBase data);
    string DoCopySpeechPart(SpeechPartToggleItem speechPartToggleItem);
}