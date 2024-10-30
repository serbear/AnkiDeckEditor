using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services.FieldsCopy;

public abstract class BaseCopyStrategy : ICopyStrategy
{
    public virtual string DoCopyCollection<T>(ObservableCollection<T> data)
    {
        var result = Common.ProcessCollection(data);
        return result;
    }

    public virtual string DoCopyString(string data)
    {
        throw new System.NotImplementedException();
    }

    public virtual string DoCopyList(List<string> data)
    {
        throw new System.NotImplementedException();
    }

    public virtual string DoCopyValueTuple((string, List<int>) data)
    {
        var result = Common.ProcessTuple(data);

        result = new StringManipulator(result).FixDotPunctuation().ResultString;
        return result;
    }

    public virtual string DoCopyWordForms(WordFormsCollectionBase data)
    {
        throw new System.NotImplementedException();
    }

    public virtual string DoCopySpeechPart(SpeechPartToggleItem speechPartToggleItem)
    {
        throw new System.NotImplementedException();
    }
}