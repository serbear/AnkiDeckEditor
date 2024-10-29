using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

public class VocabularyEntryCopyStrategy : BaseCopyStrategy
{
    // public string DoCopyCollection<T>(ObservableCollection<T> data)
    // {
    //     throw new System.NotImplementedException();
    // }

    public override string DoCopyString(string data)
    {
        return data;
    }

    // public string DoCopyList(List<string> data)
    // {
    // throw new System.NotImplementedException();
    // }

    // public string DoCopyValueTuple((string, List<int>) data)
    // {
    // throw new System.NotImplementedException();
    // }

    // public string DoCopyWordForms(WordFormsCollectionBase data)
    // {
    // throw new System.NotImplementedException();
    // }
}