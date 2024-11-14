using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AnkiDeckEditor.Controls;
using AnkiDeckEditor.Models;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace AnkiDeckEditor.Services;

public static class FieldHelper
{
    /// <summary>
    /// Determines whether there is data in the text fields of controls of type <c>T</c>.
    /// </summary>
    /// <param name="parentControl">A container that searches for <c>T</c> type controls containing text fields that
    /// will check for the contents of the data.</param>
    /// <typeparam name="T"><c>UserControl</c> contains a <c>TextBox</c> control.</typeparam>
    /// <returns>True - if there is a text in at least one of the text fields of a control of type <c>T</c>.
    /// False - if all text fields of controls of type <c>T</c> have no text.</returns>
    /// <remarks>The contents of the text fields are used to determine if there is unsaved data.
    /// If the text field contains data, it is assumed that no data has been saved.</remarks>
    public static bool IsUnsavedContent<T>(Control parentControl) where T : Control
    {
        var childrenControls = FindChildren<T>(parentControl);

        // Search for the “MainTextBox” text field of the PasteTextBox control.
        // In the current implementation, PasteTextBox has only one text field.
        // Therefore, you can search by control type.
        return childrenControls
            .Select(child => FindChildren<TextBox>(child).ToList()[0].Text)
            .Any(textBoxText => !string.IsNullOrEmpty(textBoxText));
    }

    public static bool IsFieldsChanged<T>(Control parentControl, EstonianCardRecord vocabularyCard) where T : Control
    {
        var childrenControls = FindChildren<T>(parentControl);
        // Remove duplicates.
        childrenControls = new HashSet<T>(childrenControls);

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var control in childrenControls)
        {
            var savedEntityValue = GetPropertyValueByName(vocabularyCard, control.Name);
            var currentValue = GetPropertyValueByName(parentControl.DataContext, control.Name);
            if (savedEntityValue == currentValue) continue;

            return true;
        }

        return false;
    }

    public static bool IsCheckboxFieldsChanged<T>(Control parentControl, EstonianCardRecord vocabularyCard)
        where T : Control
    {
        if (typeof(T) != typeof(CheckBox))
            throw new InvalidOperationException("The generic method must be called with the 'CheckBox' type.");

        var childrenControls = FindChildren<T>(parentControl);
        // Remove duplicates.
        childrenControls = new HashSet<T>(childrenControls);

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var control in childrenControls)
        {
            var savedEntityValue = GetPropertyValueByName(vocabularyCard, control.Name);
            var currentValue = GetPropertyValueByName(parentControl.DataContext, control.Name);

            if (savedEntityValue == currentValue) continue;


            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns a collection of child controls of type <c>T</c> in a <c>parent</c> container.
    /// </summary>
    private static IEnumerable<T> FindChildren<T>(Control? parent) where T : Control
    {
        if (parent == null) yield break;

        foreach (var child in parent.GetLogicalChildren())
        {
            if (child is T target) yield return target;

            // Recursively search the child controls.
            foreach (var descendant in FindChildren<T>((Control)child)) yield return descendant;
        }
    }

    public static void ClearTextFields<T>(Control? parentControl) where T : Control
    {
        var childrenControls = FindChildren<T>(parentControl);
        foreach (var childControl in childrenControls)
            FindChildren<TextBox>(childControl).ToList()[0].Text = string.Empty;
    }

    public static void ResetCheckBoxFields(Control? parentControl)
    {
        var childrenControls = FindChildren<CheckBox>(parentControl);
        foreach (var childControl in childrenControls)
            childControl.IsChecked = false;
    }

    public static Control GetChildren<T>(Control? parent, string controlName) where T : Control
    {
        var children = FindChildren<T>(parent);
        return children.First(c => c.Name!.Equals(controlName));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static List<T> GetChildrenList<T>(object? parent) where T : Control
    {
        return FindChildren<T>(parent as Control).ToList();
    }

    public static void RestoreCheckBoxes(Control? parentControl, List<string>? valueCollection)
    {
        // Skip if the value collection is empty.
        if ((valueCollection == null) | (valueCollection!.Count == 0)) return;

        var checkBoxes = GetChildrenList<CheckBox>(parentControl).Where(cb => cb.Content != null).ToList();

        if (checkBoxes == null)
            throw new InvalidOperationException("There are no {speech part government} check boxes.");

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var checkBoxText in valueCollection)
        {
            var checkBox = checkBoxes.FirstOrDefault(b => b.Content!.Equals(checkBoxText));

            if (checkBox != null) checkBox.IsChecked = true;
            else throw new InvalidOperationException("The speech part government check box not found.");
        }
    }

    public static void RestoreCheckBoxes(Control? parentControl, SpeechPartToggleItem? speechPartToggleItem)
    {
        // Skip if the value collection is empty.
        if (speechPartToggleItem == null) return;

        var checkBoxes = GetChildrenList<CheckBox>(parentControl);

        if (checkBoxes == null) throw new InvalidOperationException("There are no {speech part} check boxes.");

        var checkBox = checkBoxes.FirstOrDefault(b => b.Content!.Equals(speechPartToggleItem.Title));

        if (checkBox != null) checkBox.IsChecked = true;
        else throw new InvalidOperationException("The speech part check box not found.");
    }

    public static List<int> GetContextSelectedWordIndexes(ObservableCollection<ContextToggleItem> collection)
    {
        // The index of the selected word in the collection is stored in the record.
        // It is possible that there may be several identical words in a sentence, but only one of them is highlighted.
        return collection.Select(e => e.IsChecked ? collection.IndexOf(e)-1 : -1).Where(e => e >= 0).ToList();
    }

    public static void CheckContextSelectedWords(
        List<int>? indexesCollection,
        ObservableCollection<ContextToggleItem> targetCollection)
    {
        if (indexesCollection == null || indexesCollection.Count == 0) return;

        foreach (var idx in indexesCollection) targetCollection[idx].IsChecked = true;
    }

    public static string GetFieldValue(object parent, string fieldName)
    {
        // Get this class type.
        var type = parent.GetType();

        // FieldInfo field;
        PropertyInfo field;

        try
        {
            // ReSharper disable once GrammarMistakeInComment
            // Expression 'f.Name[1..]' means: skip the "$" symbol in the name of the class field.
            // field = type.GetRuntimeFields().First(f => f.Name[1..].Equals(fieldName));
            field = type.GetRuntimeProperties().First(f => f.Name.Equals(fieldName));
        }
        catch (Exception e) when (e is ArgumentNullException or InvalidOperationException)
        {
            throw new ArgumentException($"Field with the name '{fieldName}' is not found.");
        }

        // If the field is public and exists, return its value.
        return (string)field.GetValue(parent)!;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static string GetPropertyValueByName(object? parent, string? propertyName)
    {
        if (parent == null) throw new ArgumentException("Argument 'parent' cannot be null.");
        if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("Argument 'propertyName' cannot be null.");

        // Get this class type.
        var type = parent.GetType();

        var field = type.GetProperty(propertyName);
        if (field == null) throw new ArgumentException($"Property with the name '{propertyName}' is not found.");

        return (string)field.GetValue(parent)!;
    }
}