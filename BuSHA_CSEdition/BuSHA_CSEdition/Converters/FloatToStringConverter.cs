using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BuSHA_CSEdition.Converters;

public class FloatToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is float.NaN)
            return "";
        if (value is float floatValue)
        {
            return floatValue.ToString(CultureInfo.InvariantCulture);
        }
        return "0";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue && float.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            return result;
        }
        return float.NaN; // Default if parsing fails
    }
}