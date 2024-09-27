using System.Collections.Generic;

namespace AnkiDeckEditor.Services.FieldsCopy;

public abstract class WordFormsCollectionBase()
{
    public List<string> WordFormsCollection { get; set; }

    protected WordFormsCollectionBase(List<string> value) : this()
    {
        WordFormsCollection = value;
    }
}

public class NonVerbWordFormCollection(List<string> value) : WordFormsCollectionBase(value);

public class VerbWordFormCollection(List<string> value) : WordFormsCollectionBase(value);