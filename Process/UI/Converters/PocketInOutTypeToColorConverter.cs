using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Process.Models.Pocket;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PocketInOutTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = Application.Current.FindResource("SoftBlueBrush") as SolidColorBrush;

            if (value != null && value is PocketInOutType val && val == PocketInOutType.In)
            {
                path = Application.Current.FindResource("LightYellowBrush") as SolidColorBrush;
            }

            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}