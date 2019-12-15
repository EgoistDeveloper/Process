using Process.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Process.UI.Controls
{
    /// <summary>
    /// Interaction logic for InfoCard.xaml
    /// </summary>
    public partial class InfoCard : UserControl
    {
        public InfoCard()
        {
            InitializeComponent();
        }

        #region Base Properties

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(InfoCard),
                new UIPropertyMetadata(new SolidColorBrush(Colors.White)));

        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InfoCard),
                new UIPropertyMetadata(new CornerRadius(5)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region Icon Properties

        public static readonly DependencyProperty IconBackgroundColorProperty =
            DependencyProperty.Register("IconBackgroundColor", typeof(SolidColorBrush), typeof(InfoCard),
                new UIPropertyMetadata(new SolidColorBrush(Colors.IndianRed)));

        public SolidColorBrush IconBackgroundColor
        {
            get => (SolidColorBrush)GetValue(IconBackgroundColorProperty);
            set => SetValue(IconBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty IconPaddingProperty =
            DependencyProperty.Register("IconPadding", typeof(Thickness), typeof(InfoCard),
                new UIPropertyMetadata(new Thickness(15)));

        public Thickness IconPadding
        {
            get => (Thickness)GetValue(IconPaddingProperty);
            set => SetValue(IconPaddingProperty, value);
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(InfoCard),
                new UIPropertyMetadata(50.0));

        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(InfoCard),
                new UIPropertyMetadata(50.0));

        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(InfoCard),
                new UIPropertyMetadata(new SolidColorBrush(Colors.White)));

        public SolidColorBrush IconColor
        {
            get => (SolidColorBrush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(Geometry), typeof(InfoCard),
                new UIPropertyMetadata(Geometry.Parse("M12,2L1,21H23M12,6L19.53,19H4.47")));

        public Geometry IconData
        {
            get => (Geometry)GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

        #endregion

        #region Info Text 1 Properties

        public static readonly DependencyProperty InfoText1Property =
            DependencyProperty.Register("InfoText1", typeof(string), typeof(InfoCard),
                new UIPropertyMetadata("Info Text 1"));

        public string InfoText1
        {
            get => (string)GetValue(InfoText1Property);
            set => SetValue(InfoText1Property, value);
        }

        public static readonly DependencyProperty InfoText1FontSizeProperty =
            DependencyProperty.Register("InfoText1FontSize", typeof(double), typeof(InfoCard),
                new UIPropertyMetadata(20.0));

        public double InfoText1FontSize
        {
            get => (double)GetValue(InfoText1FontSizeProperty);
            set => SetValue(InfoText1FontSizeProperty, value);
        }

        public static readonly DependencyProperty InfoText1FontWeightProperty =
            DependencyProperty.Register("InfoText1FontWeight", typeof(FontWeight), typeof(InfoCard));

        public FontWeight InfoText1FontWeight
        {
            get => (FontWeight)GetValue(InfoText1FontWeightProperty);
            set => SetValue(InfoText1FontWeightProperty, value);
        }

        #endregion

        #region Info Text 2 Properties

        public static readonly DependencyProperty InfoText2Property =
            DependencyProperty.Register("InfoText2", typeof(string), typeof(InfoCard),
                new UIPropertyMetadata("Info Text 2"));

        public string InfoText2
        {
            get => (string)GetValue(InfoText2Property);
            set => SetValue(InfoText2Property, value);
        }

        public static readonly DependencyProperty InfoText2FontSizeProperty =
            DependencyProperty.Register("InfoText2FontSize", typeof(double), typeof(InfoCard),
                new UIPropertyMetadata(20.0));

        public double InfoText2FontSize
        {
            get => (double)GetValue(InfoText2FontSizeProperty);
            set => SetValue(InfoText2FontSizeProperty, value);
        }

        public static readonly DependencyProperty InfoText2FontWeightProperty =
            DependencyProperty.Register("InfoText2FontWeight", typeof(FontWeight), typeof(InfoCard));

        public FontWeight InfoText2FontWeight
        {
            get => (FontWeight)GetValue(InfoText2FontWeightProperty);
            set => SetValue(InfoText2FontWeightProperty, value);
        }


        #endregion


        public static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(string), typeof(InfoCard));

        public string Tooltip
        {
            get => (string)GetValue(TooltipProperty);
            set => SetValue(TooltipProperty, value);
        }
    }
}
