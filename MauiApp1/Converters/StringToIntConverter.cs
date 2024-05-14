using System.Globalization;

namespace MyGarage.Converters;

public class StringToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int output;

        if ((string) value == "" || !int.TryParse((string)value, out output))
        {
            return 0;
        }

        return output;
    }
}
