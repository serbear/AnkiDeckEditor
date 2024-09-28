using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AnkiDeckEditor.Converters;

public class SumControlHeightConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        Console.WriteLine("dd");

        var height = 0.0;

        foreach (var h in values)
            if (h is double d)
                height += d;

        return height;
    }
}