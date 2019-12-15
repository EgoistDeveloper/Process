using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using Process.Models.Pocket;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PocketInOutTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = Application.Current.FindResource("ArrowBottomLeftThick") as Path;

            if (value != null && value is PocketInOutType val && val == PocketInOutType.In)
            {
                path = Application.Current.FindResource("ArrowTopRightThick") as Path;
            }

            return path.Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}