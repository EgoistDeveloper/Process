using Process.Models.Dashboard;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Process.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class WeatherDescriptionToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = "WeatherSunny";

            if (value != null && value is Description val)
            {
                switch (val)
                {
                    case Description.BrokenClouds:
                        path = "WeatherPartlyCloudy";
                        break;
                    case Description.ClearSky:
                        path = "WeatherSunny";
                        break;
                    case Description.FewClouds:
                        path = "WeatherCloudy";
                        break;
                    case Description.LightRain:
                        path = "WeatherPartlyRainy";
                        break;
                    case Description.OvercastClouds:
                        path = "WeatherFog";
                        break;
                    case Description.ScatteredClouds:
                        path = "WeatherHazy";
                        break;
                    default:
                        path = "WeatherSunny";
                        break;
                }
            }

            return (Application.Current.FindResource(path) as Path).Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
