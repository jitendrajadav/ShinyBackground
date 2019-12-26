using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace KegID.Converter
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToLast8CharacterConverter : IValueConverter
    {
        public static StringToLast8CharacterConverter Instance { get; } = new StringToLast8CharacterConverter();

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTimeOffset.UtcNow;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var text = ((string)value);

            return Regex.Match(text, @"(.{8})\s*$").Value.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
