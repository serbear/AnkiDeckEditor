using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace AnkiDeckEditor.Controls;

public partial class SpeechGovernmentFilter : UserControl
{
    private char[] _letters;

    public static readonly DirectProperty<SpeechGovernmentFilter, char[]> LettersProperty =
        AvaloniaProperty.RegisterDirect<SpeechGovernmentFilter, char[]>(
            "Letters", o => o.Letters, (o, v) => o.Letters = v);

    private char[] _SelectedLetters;

    public static readonly DirectProperty<SpeechGovernmentFilter, char[]> SelectedLettersProperty =
        AvaloniaProperty.RegisterDirect<SpeechGovernmentFilter, char[]>(
            "SelectedLetters", o => o.SelectedLetters, (o, v) => o.SelectedLetters = v);

    public SpeechGovernmentFilter()
    {
        InitializeComponent();
    }

    public char[] Letters
    {
        get => _letters;
        set
        {
            SetAndRaise(LettersProperty, ref _letters, value);
            UpdateFilterButtonCollection(_letters);
        }
    }

    public char[] SelectedLetters
    {
        get => _SelectedLetters;
        set => SetAndRaise(SelectedLettersProperty, ref _SelectedLetters, value);
    }

    private void UpdateFilterButtonCollection(IEnumerable<char> letters)
    {
        foreach (var letter in letters)
            SpeechGovernmentFilterToggleButtons.Items.Add(letter);
    }

    private readonly List<char> _toggledLetters = [];

    private void SpeechPartGovernmentLetterToggle_OnClick(object? sender, RoutedEventArgs e)
    {
        var buttonObj = sender as ToggleButton;
        var letter = char.ToLower((char)buttonObj?.Content!);

        if ((bool)buttonObj.IsChecked!) _toggledLetters.Add(letter);
        else _toggledLetters.Remove(letter);

        _SelectedLetters = _toggledLetters.ToArray();

        LetterChanged(this, new LetterChangedEventArgs(_SelectedLetters));
    }

    // -------

    // Событие LetterChanged
    public event EventHandler<LetterChangedEventArgs> LetterChanged;

    // Пример свойств для привязки
    public static readonly StyledProperty<string> LettersCollectionProperty =
        AvaloniaProperty.Register<SpeechGovernmentFilter, string>(nameof(LettersCollection));

    public string LettersCollection
    {
        get => GetValue(LettersCollectionProperty);
        set => SetValue(LettersCollectionProperty, value);
    }

    // Пример того, как можно вызвать событие
    // private void OnLettersChanged()
    // {
    //     LetterChanged(this, EventArgs.Empty);
    // }

    // Пример, как использовать это событие
    // public SpeechGovernmentFilter()
    // {
    //     this.PropertyChanged += (sender, args) =>
    //     {
    //         if (args.Property == LettersProperty)
    //         {
    //             OnLettersChanged(); // Вызов события при изменении свойства
    //         }
    //     };
    // }
}

public class LetterChangedEventArgs(char[] letters) : EventArgs
{
    public char[] Letters { get; } = letters;
}