using System;
using System.Text.Json.Serialization;

namespace AnkiDeckEditor.Models;

public class SpeechPartToggleItem : ToggleItem
{
    public string? Translation { get; }

    // public bool IsVerb { get; }
    public VerbTypes? VerbType { get; set; }

    // ReSharper disable once UnusedMember.Global
    public string VerbTypeName { get; }

    // public SpeechPartToggleItem(string? title, string? translation, bool isChecked) : base(title, isChecked)
    // {
    //     Translation = translation;
    //     IsVerb = false;
    //     VerbType = null;
    // }
    //
    // public SpeechPartToggleItem(string? title, string? translation, VerbTypes verbType, bool isChecked)
    //     : base(title, isChecked)
    // {
    //     Translation = translation;
    //     VerbType = verbType;
    // }

    [JsonConstructor]
    public SpeechPartToggleItem(string? title, string? translation, string? verbTypeName, bool isChecked)
        : base(title, isChecked)
    {
        Translation = translation;
        if (verbTypeName != null)
            VerbType = (VerbTypes)Enum.Parse(typeof(VerbTypes), verbTypeName);
    }
}