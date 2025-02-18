using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace AnkiDeckEditor.Converters;

public class SpeechGovernmentFilterConverter : IMultiValueConverter
{
    public object? Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count == 0) return AvaloniaProperty.UnsetValue;

        try
        {
            // var totalWidth = values.OfType<double>().Sum();
            return 0;
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}