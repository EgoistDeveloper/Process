using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Process.UI.Converters
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ExpiredDateToReverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                if (parameter != null)
                {
                    int.TryParse((string)parameter, out int addDays);
                    return !(DateTime.Now.Date > date.AddDays(addDays));
                }

                return !(DateTime.Now.Date > date);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}