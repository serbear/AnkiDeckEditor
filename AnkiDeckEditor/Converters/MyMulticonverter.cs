using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace AnkiDeckEditor.Converters;

public class MyMulticonverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        Console.WriteLine(values.ToList()[0]);
        return values;
    }
}