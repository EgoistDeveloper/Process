using GalaSoft.MvvmLight;
using Microsoft.EntityFrameworkCore;
using Process.Data;
using Process.Dialogs.Calendar;
using Process.Helpers;
using Process.Models.Calendar;
using Process.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Process.ViewModel.Calendar
{
    public class CalendarViewModel : ViewModelBase
    {
        public CalendarViewModel()
        {
            LoadYears()
            .ContinueWith(async T => await CreateCalendarItems()
            .ContinueWith(async T => await LoadLogOfDays()));

            YearChangedCommand = new RelayCommand(async p => await CreateCalendarItems()
            .ContinueWith(async T => await LoadLogOfDays()));
            CalendarButtonCommand = new RelayParameterizedCommand(CalendarButtonAction);
        }

        #region Commands

        public ICommand YearChangedCommand { get; set; }
        public ICommand CalendarButtonCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Selected year
        /// </summary>
        public Year TargetYear { get; set; } = new Year();

        /// <summary>
        /// List of years
        /// </summary>
        public List<Year> Years { get; set; } = new List<Year>();

        /// <summary>
        /// Months of the selected year
        /// </summary>
        public ObservableCollection<CalendarMonth> Months { get; set; } = new ObservableCollection<CalendarMonth>();

        #endregion

        #region Methods

        /// <summary>
        /// Loads years from the birth year
        /// TODO change birth date
        /// </summary>
        public async Task LoadYears()
        {
            //if (AppSession.User != null && !string.IsNullOrEmpty(AppSession.User.BirthDate))
            //{
            //}

            var birthDateTime = new DateTime(1998, 2, 27);
            //if (DateTime.TryParse(AppSession.User.BirthDate, Settings.CultureInfo, DateTimeStyles.None, out DateTime birthDateTime))
            if (birthDateTime != null)
            {
                var currentYear = DateTime.Now.Year;

                if (birthDateTime.Year < currentYear)
                {
                    for (int i = 0; i < (currentYear - birthDateTime.Year) + 1; i++)
                    {
                        Years.Add(new Year()
                        {
                            YearNumber = birthDateTime.Year + i
                        });
                    }

                    Years = Years.OrderByDescending(x => x.YearNumber).ToList();
                    TargetYear = Years.First();

                    return;
                }
                else if (birthDateTime.Year > currentYear)
                {
                    MessageBox.Show("Birth year bigger than curren year, check your birth date and system date time settings");
                    return;
                }
            }

            // default year = current year
            TargetYear.YearNumber = DateTime.Now.Year;
            Years.Add(TargetYear);
        }

        /// <summary>
        /// Calendar item and sub item creator; months ans days
        /// </summary>
        public async Task CreateCalendarItems()
        {
            var months = new ObservableCollection<CalendarMonth>();

            for (int i = 1; i < 13; i++)
            {
                var days = new List<CalendarDay>();

                var firstDayOfMonth = new DateTime(TargetYear.YearNumber, i, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var lastDayLastMonth = firstDayOfMonth.AddDays(-1);

                var previousCoundown = PreviousNextMonthDays.GetPreviousCountdown(firstDayOfMonth.DayOfWeek);
                var nextCoundown = PreviousNextMonthDays.GetNextCountdown(lastDayOfMonth.DayOfWeek);

                // previous month days
                for (int ii = previousCoundown - 1; ii > -1; ii--)
                {
                    days.Add(new CalendarDay 
                    { 
                        Date = new DateTime(TargetYear.YearNumber, i - 1 < 2 ? 1 : i - 1, lastDayLastMonth.Day - ii),
                        DayStatus = DayStatus.DayOfPreviousMonth
                    });
                }

                // this month days
                var thisMonthDayCount = DateTime.DaysInMonth(TargetYear.YearNumber, i);

                for (int ii = 1; ii < thisMonthDayCount + 1; ii++)
                {
                    var date = new DateTime(TargetYear.YearNumber, i, ii);

                    days.Add(new CalendarDay
                    {
                        IsEnabled = true,
                        IsDefault = date != DateTime.Now ? false : true,
                        Date = date,
                        DayStatus = date != DateTime.Now ? DayStatus.EmptyDay : DayStatus.EmptyToday
                    });
                }

                // next month days
                var daysBlockCount = previousCoundown + nextCoundown + thisMonthDayCount;

                if (daysBlockCount < 42)
                {
                    nextCoundown += 42 - daysBlockCount;
                }

                for (int ii = 1; ii < nextCoundown + 1; ii++)
                {
                    days.Add(new CalendarDay
                    {
                        Date = new DateTime(TargetYear.YearNumber, i + 1 > 12 ? 12 : i + 1, ii),
                        DayStatus = DayStatus.DayOfNextMonth
                    });
                }

                months.Add(new CalendarMonth 
                {
                    MonthNumber = i,
                    MonthName = Settings.CultureInfo.DateTimeFormat.GetMonthName(i),
                    Year = TargetYear.YearNumber,
                    CalendarDays = days
                });
            }

            Months = months;
        }

        /// <summary>
        /// Retrives diary logs by target year and applies month of days
        /// </summary>
        public async Task LoadLogOfDays()
        {
            using var db = new AppDbContext();

            // diary logs
            var diaryLogs = db.CalendarLogs.Where(x => EF.Functions.Like(x.Date.Year.ToString(), TargetYear.YearNumber.ToString()))
            .Select(x => new CalendarLog
            {
                Id = x.Id,
                Date = x.Date,
                AddedDate = x.AddedDate,
                Activity = x.Activity,
                IsDone = x.IsDone,
                Description = x.Description
            }).ToList();

            // set diarylogs of days
            foreach (CalendarLog log in diaryLogs)
            {
                var targetMonth = log.Date.Month - 1;

                if (Months[targetMonth].CalendarDays.Any(x => x.Date == log.Date))
                {
                    var day = Months[targetMonth].CalendarDays.First(x => x.Date == log.Date);
                    day.CalendarLog = log;
                    day.DayStatus = log.Date.Date == DateTime.Now.Date ? DayStatus.LoggedToday : DayStatus.LoggedDay;
                }
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Calendar button event for add or view log
        /// </summary>
        /// <param name="sender"></param>
        public void CalendarButtonAction(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is CalendarDay day)) return;

            var dialog = new CalendarLogDialog();

            dialog.Closing += (s, a) =>
            {
                if (dialog.DataContext is CalendarLogViewModel vm && vm.CalendarLog.Activity.Length > 0)
                {
                    day.DayStatus = day.Date == DateTime.Now ? DayStatus.LoggedToday : DayStatus.LoggedDay;
                    day.CalendarLog = vm.CalendarLog;

                    for (int i = 0; i < Months.Count; i++)
                    {
                        if (Months[i].MonthNumber == day.Date.Month)
                        {
                            for (int ii = 0; ii < Months[i].CalendarDays.Count; ii++)
                            {
                                if (Months[i].CalendarDays[ii].Date == day.Date)
                                    Months[i].CalendarDays[ii] = day;
                                return;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new CalendarLogViewModel(dialog, day.Date, day.CalendarLog));
        }



        #endregion
    }
}
