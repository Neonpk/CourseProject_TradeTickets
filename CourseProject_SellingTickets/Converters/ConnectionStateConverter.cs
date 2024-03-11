using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CourseProject_SellingTickets.Converters;

public class ConnectionStateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string param = parameter != null ? (string)parameter : String.Empty;
        bool isConnected = (bool)value!;
        
        if (isConnected)
            return param.Equals("String") ? "Соединение с БД установлено." : param.Equals("Foreground") ? Brush.Parse("#252525") : true;
        
        return param.Equals("String") ? "Не удалось соединиться с БД." : param.Equals("Foreground") ? Brushes.Firebrick : false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}