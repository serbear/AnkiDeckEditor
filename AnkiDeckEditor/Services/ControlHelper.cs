using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;

namespace AnkiDeckEditor.Services;

public class ControlHelper
{
    public static void SetControlReference(object obj, string fieldName, Control? control)
    {
        // Check argument values.
        if (obj == null) throw new InvalidOperationException("The 'obj' argument cannot be null.");
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new InvalidOperationException("The 'fieldName' argument cannot be null.");
        if (control == null) throw new InvalidOperationException("The 'control' argument cannot be null.");

        var objType = obj.GetType();

        // Find the field by its name.
        var field = objType.GetRuntimeProperties().First(f => f.Name.Equals(fieldName));

        // Throw error if the field is not found.
        if (field == null) throw new InvalidOperationException($"The field with name '{fieldName}' not found.");

        field.SetValue(obj, control);
    }
}