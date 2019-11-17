using Process.UI.Controls.Common.Calendar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Process.UI.Controls.Common.Calendar
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        public Calendar()
        {
            InitializeComponent();

            CreateDayItems();
        }

        //ObservableCollection<Day> Days = new ObservableCollection<Day>();

        #region Base Properties

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(Calendar),
                new UIPropertyMetadata(new SolidColorBrush(Colors.White)));

        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Calendar),
                new UIPropertyMetadata(new CornerRadius(5)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CalendarMarginProperty =
            DependencyProperty.Register("CalendarMargin", typeof(Thickness), typeof(Calendar),
                new UIPropertyMetadata(new Thickness(5)));

        public Thickness CalendarMargin
        {
            get => (Thickness)GetValue(CalendarMarginProperty);
            set => SetValue(CalendarMarginProperty, value);
        }

        public static readonly DependencyProperty CalendarPaddingProperty =
            DependencyProperty.Register("CalendarPadding", typeof(Thickness), typeof(Calendar),
                new UIPropertyMetadata(new Thickness(5)));

        public Thickness CalendarPadding
        {
            get => (Thickness)GetValue(CalendarPaddingProperty);
            set => SetValue(CalendarPaddingProperty, value);
        }

        public static readonly DependencyProperty CalendarShadowOpacityProperty =
            DependencyProperty.Register("CalendarShadowOpacity", typeof(double), typeof(Calendar),
                new UIPropertyMetadata(0.02));

        public double CalendarShadowOpacity
        {
            get => (double)GetValue(CalendarShadowOpacityProperty);
            set => SetValue(CalendarShadowOpacityProperty, value);
        }

        #region CultureInfo

        public static readonly DependencyProperty CultureInfoProperty =
            DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(Calendar),
                new PropertyMetadata(new CultureInfo("en-US"), OnCultureInfoCallBack));

        public CultureInfo CultureInfo
        {
            get => (CultureInfo)GetValue(CultureInfoProperty);
            set => SetValue(CultureInfoProperty, value);
        }

        private static void OnCultureInfoCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = sender as Calendar;
            c.MonthName = c.CultureInfo.DateTimeFormat.GetMonthName(c.MonthNumber);
        }

        #endregion

        #endregion

        #region Calendar

        #region YearNumber

        public static readonly DependencyProperty YearNumberProperty =
            DependencyProperty.Register("YearNumber", typeof(int), typeof(Calendar),
                new UIPropertyMetadata(DateTime.Now.Year));

        public int YearNumber
        {
            get => (int)GetValue(YearNumberProperty);
            set => SetValue(YearNumberProperty, value);
        }

        #endregion

        #region MonthName

        public static readonly DependencyProperty MonthNameProperty =
            DependencyProperty.Register("MonthName", typeof(string), typeof(Calendar),
                new UIPropertyMetadata("January"));

        public string MonthName
        {
            get => (string)GetValue(MonthNameProperty);
            set => SetValue(MonthNameProperty, value);
        }

        #endregion

        #region MonthNumber

        public static readonly DependencyProperty MonthNumberProperty =
            DependencyProperty.Register("MonthNumber", typeof(int), typeof(Calendar),
                new UIPropertyMetadata(1, OnMonthNumberCallBack),
                new ValidateValueCallback(IsValidMonthNumber));

        public int MonthNumber
        {
            get => (int)GetValue(MonthNumberProperty);
            set => SetValue(MonthNumberProperty, value);
        }

        private static void OnMonthNumberCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = sender as Calendar;
            c.MonthName = c.CultureInfo.DateTimeFormat.GetMonthName(c.MonthNumber);

            c.CreateDayItems();
        }

        private static bool IsValidMonthNumber(object value)
        {
            return value is int val ? val >= 1 && val <= 12 : false;
        }

        #endregion

        #region DayDataTemplate

        public static readonly DependencyProperty DayDataTemplateProperty =
            DependencyProperty.Register("DayDataTemplate", typeof(DataTemplate), typeof(Calendar));

        public DataTemplate DayDataTemplate
        {
            get => (DataTemplate)GetValue(DayDataTemplateProperty);
            set => SetValue(DayDataTemplateProperty, value);
        }

        #endregion

        public static readonly DependencyProperty DaysProperty =
            DependencyProperty.Register("Days", typeof(ObservableCollection<Day>), typeof(Calendar));

        public ObservableCollection<Day> Days
        {
            get => (ObservableCollection<Day>)GetValue(DaysProperty);
            set => SetValue(DaysProperty, value);
        }

        public static readonly DependencyProperty CalendarButtonCommandProperty =
            DependencyProperty.Register("CalendarButtonCommand", typeof(ICommand), typeof(Calendar));

        public ICommand CalendarButtonCommand
        {
            get => (ICommand)GetValue(CalendarButtonCommandProperty);
            set => SetValue(CalendarButtonCommandProperty, value);
        }

        #endregion


        #region Helper Methods

        private void CreateDayItems()
        {
            Days = new ObservableCollection<Day>();

            var firstDayOfMonth = new DateTime(YearNumber, MonthNumber, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var lastDayLastMonth = firstDayOfMonth.AddDays(-1);

            var previousCoundown = GetPreviousCountdown(firstDayOfMonth.DayOfWeek);
            var nextCoundown = GetNextCountdown(lastDayOfMonth.DayOfWeek);

            // previous month days
            for (int i = previousCoundown - 1; i > -1; i--)
            {
                var month = MonthNumber - 1 < 2 ? 1 : MonthNumber - 1;

                var day = new Day 
                { 
                    Date = new DateTime(YearNumber, month, lastDayLastMonth.Day - i),
                    DayStatus = DayStatus.DayOfPreviousMonth
                };

                Days.Add(day);
            }

            // this month days
            var thisMonthDayCount = DateTime.DaysInMonth(YearNumber, MonthNumber);

            for (int i = 1; i < thisMonthDayCount + 1; i++)
            {
                var date = new DateTime(YearNumber, MonthNumber, i);
                var day = new Day
                {
                    Date = date,
                    IsEnabled = true,
                    DayStatus = date == DateTime.Now ? DayStatus.EmptyToday : DayStatus.EmptyDay
                };

                Days.Add(day);
            }

            // next month days
            var daysBlockCount = previousCoundown + nextCoundown + thisMonthDayCount;

            if (daysBlockCount < 42)
            {
                nextCoundown += 42 - daysBlockCount;
            }

            for (int i = 1; i < nextCoundown + 1; i++)
            {
                var month = MonthNumber + 1 > 12 ? 12 : MonthNumber + 1;

                var day = new Day
                {
                    Date = new DateTime(YearNumber, month, i),
                    DayStatus = DayStatus.DayOfNextMonth
                };

                Days.Add(day);
            }
        }

        /// <summary>
        /// Previous month's last days countdown
        /// </summary>
        /// <param name="dayOfWeek">Day of week of first day of the current month.</param>
        /// <returns></returns>
        public static int GetPreviousCountdown(DayOfWeek dayOfWeek)
        {
            var previousCoundown = 7;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    previousCoundown = 7;
                    break;
                case DayOfWeek.Tuesday:
                    previousCoundown = 1;
                    break;
                case DayOfWeek.Wednesday:
                    previousCoundown = 2;
                    break;
                case DayOfWeek.Thursday:
                    previousCoundown = 3;
                    break;
                case DayOfWeek.Friday:
                    previousCoundown = 4;
                    break;
                case DayOfWeek.Saturday:
                    previousCoundown = 5;
                    break;
                case DayOfWeek.Sunday:
                    previousCoundown = 6;
                    break;
                default:
                    previousCoundown = 7;
                    break;
            }

            return previousCoundown;
        }

        /// <summary>
        /// Next month's first days countdown
        /// </summary>
        /// <param name="dayOfWeek">Day of week of last day of the current month.</param>
        /// <returns></returns>
        public static int GetNextCountdown(DayOfWeek dayOfWeek)
        {
            var nextCoundown = 7;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    nextCoundown = 6;
                    break;
                case DayOfWeek.Tuesday:
                    nextCoundown = 5;
                    break;
                case DayOfWeek.Wednesday:
                    nextCoundown = 4;
                    break;
                case DayOfWeek.Thursday:
                    nextCoundown = 3;
                    break;
                case DayOfWeek.Friday:
                    nextCoundown = 2;
                    break;
                case DayOfWeek.Saturday:
                    nextCoundown = 1;
                    break;
                case DayOfWeek.Sunday:
                    nextCoundown = 7;
                    break;
                default:
                    nextCoundown = 7;
                    break;
            }

            return nextCoundown;
        }

        #endregion
    }
}
