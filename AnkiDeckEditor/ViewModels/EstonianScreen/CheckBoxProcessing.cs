using System;
using System.Collections.Generic;
using System.Linq;
using AnkiDeckEditor.Enums;
using Avalonia.Controls;

namespace AnkiDeckEditor.ViewModels.EstonianScreen;

public partial class EstonianScreenViewModel
{
    public void OnCheckboxChanged(CheckBox? sender, StrategyNames strategyName)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (strategyName)
        {
            case StrategyNames.SpeechPart:
                OnSpeechPartCheckboxChanged(sender);
                break;
            case StrategyNames.SpeechPartGovernment:
                OnSpeechPartGovernmentCheckboxChanged(sender);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategyName), strategyName, null);
        }
    }

    private void OnSpeechPartCheckboxChanged(CheckBox? sender)
    {
        if (sender == null) throw new Exception("There is no checkbox object.");
        if (_currentEditCard == null) return;

        List<bool> condition =
        [
            // The selected part of speech corresponds to the part of speech being edited.
            _currentEditCard.SpeechPart.Equals(sender.Content),
            // The checkbox is checked.
            sender.IsChecked == true
        ];

        IsSaveEntityListButtonEnabled = !condition.All(e => e);
    }

    private void OnSpeechPartGovernmentCheckboxChanged(CheckBox? sender)
    {
        if (sender == null) throw new Exception("There is no checkbox object.");
        if (_currentEditCard == null) return;

        var currentGovernment = VerbControlItems.Where(e => e.IsChecked).Select(e => e.Title).ToList();

        // Update the speech part government list.
        if (sender.IsChecked.Equals(true)) currentGovernment.Add(sender.Content as string);
        else currentGovernment.Remove(sender.Content as string);

        // Sort lists before equality checking.
        currentGovernment.Sort();
        _currentEditCard.SpeechPartGovernment.Sort();

        IsSaveEntityListButtonEnabled = !currentGovernment.SequenceEqual(_currentEditCard.SpeechPartGovernment);
    }
}