using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using CourseProject_SellingTickets.Helpers;

namespace CourseProject_SellingTickets.Converters;

public class ImageSourceWebConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Task.Run(async () => await ImageHelper.LoadFromWeb(new Uri((string)value!))).Result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}