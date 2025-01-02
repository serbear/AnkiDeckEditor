using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace AnkiDeckEditor.Converters;

public class SumControlHeightConverter : IMultiValueConverter
{
    public object? Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count == 0) return AvaloniaProperty.UnsetValue;

        try
        {
            var totalHeight = values.OfType<double>().Sum();
            totalHeight -= (double)values[0]!;

            return (double)values[0]! - totalHeight - 16;
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}