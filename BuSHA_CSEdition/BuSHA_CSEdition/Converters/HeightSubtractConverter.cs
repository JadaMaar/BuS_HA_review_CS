using System.Collections.Generic;

namespace BuSHA_CSEdition.Converters;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

public class HeightSubtractConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 3 && values[0] is double windowHeight && values[1] is double stackPanelHeight && values[2] is double stackPanelHeight1)
        {
            // Subtract the height of the StackPanel from the window height
            return windowHeight - stackPanelHeight - stackPanelHeight1;
        }

        return 0d;
    }
}
