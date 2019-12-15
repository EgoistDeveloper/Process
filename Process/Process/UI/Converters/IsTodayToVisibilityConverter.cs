using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class IsTodayToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                return DateTime.Now.Date == date.Date ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}