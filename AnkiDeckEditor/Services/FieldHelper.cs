using System.Collections.Generic;
using System.Linq;
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

    public static void ClearFields<T>(Control? parentControl) where T : Control
    {
        var childrenControls = FindChildren<T>(parentControl);
        foreach (var childControl in childrenControls) FindChildren<TextBox>(childControl).ToList()[0].Text = "";
    }

    public static Control GetChildren<T>(Control? parent, string controlName) where T : Control
    {
        var children = FindChildren<T>(parent);
        return children.First(c => c.Name!.Equals(controlName));
    }
}