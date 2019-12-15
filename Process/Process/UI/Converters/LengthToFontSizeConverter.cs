using System;
using System.Globalization;
using System.Windows.Data;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class LengthToFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 15;

            var length = value is long ? (long)value : (int)value;

            return new Random().Next(8, 30);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
