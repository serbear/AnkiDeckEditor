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
            var totalHeight = values
                .Skip(1)
                .OfType<double>()
                .Sum();

            return totalHeight - (double)values[0]!;
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}