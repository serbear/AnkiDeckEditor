using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace AnkiDeckEditor.Converters;

public class TextToVisibleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return (value != null) & (value.ToString()!.Length > 0);
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}