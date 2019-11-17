using Process.Models.Common;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class DayButonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DayStatus dayStatus))
                return Application.Current.FindResource("DayButtonBaseStyle") as Style;

            return dayStatus switch
            {
                DayStatus.LoggedDay => Application.Current.FindResource("LoggedDayButtonStyle") as Style,
                DayStatus.LoggedToday => Application.Current.FindResource("LoggedTodayButtonStyle") as Style,
                _ => Application.Current.FindResource("DayButtonBaseStyle") as Style,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}