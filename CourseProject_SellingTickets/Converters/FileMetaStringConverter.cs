using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class FileMetaStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FileMeta fileMeta)
        {
            return parameter is "string" ? $"Файл: {fileMeta.FileName}" :
                parameter is "foreground" ? Brushes.White : true;
        }

        return parameter is "string" ? "Файл не выбран" : parameter is "foreground" ? Brushes.Orange : false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
