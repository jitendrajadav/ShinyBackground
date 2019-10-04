using System;
using System.Globalization;
using Xamarin.Forms;

namespace KegID.Converter
{
    public class UtcToLocalDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return DateTimeOffset.Parse(value.ToString()).ToLocalTime().ToString(culture.DateTimeFormat.ShortDatePattern);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
