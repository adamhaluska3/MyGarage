﻿using MyGarage.Models;
using System.Globalization;

namespace MyGarage.Converters;

public class IntToFuelType : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (FuelType)value;
    }
}
