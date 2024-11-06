using System.Collections.ObjectModel;

namespace AnkiDeckEditor.Services.FieldsCopy;

/// <summary>
/// Implement functionality for copying the Literal Translation field data into the clipboard.
/// </summary>
public class LiteralTranslationCopyStrategy : BaseCopyStrategy
{
    public override string DoCopyCollection<T>(ObservableCollection<T> translationData)
    {
        var result = Common.ProcessCollection(translationData);
        return new StringManipulator(result).FixDotPunctuation().ResultString;
    }

    // public string DoCopyString(string data)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public string DoCopyList(List<string> data)
    // {
    //     throw new NotImplementedException();
    // }
    //
    //
    // public string DoCopyValueTuple(ValueTuple<string, List<int>> data)
    // {
    //     var result = Common.ProcessTuple(data);
    //
    //     result = new StringManipulator(result).FixDotPunctuation().ResultString;
    //     return result;
    // }
    //
    // public string DoCopyWordForms(WordFormsCollectionBase data)
    // {
    //     throw new NotImplementedException();
    // }
}