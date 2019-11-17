using Process.Models.Diet;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class UnitToShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Unit unit = value == null ? Unit.Gram : (Unit)value;

            if (unit == Unit.Gram)
            {
                return "gr";
            }
            else if (unit == Unit.Mikrogram)
            {
                return "µg";
            }
            else if (unit == Unit.Miligram)
            {
                return "mg";
            }
            else if (unit == Unit.InternationalUnit)
            {
                return "IU";
            }

            return "gr";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
