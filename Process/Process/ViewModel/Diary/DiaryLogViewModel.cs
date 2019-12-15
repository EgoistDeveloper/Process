using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using Process.Models.Common;
using Process.Models.Diary;
using Process.Data;
using Process.Helpers;
using LiveCharts;
using Process.Dialogs.Diary;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Process.ViewModel.Diary
{
    public class DiaryLogViewModel : ViewModelBase
    {
        public DiaryLogViewModel()
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
        public ICommand AddToBlackListCommand { get; set; }
        public ICommand RemoveFromBlackListCommand { get; set; }
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
        public ObservableCollection<DiaryMonth> Months { get; set; } = new ObservableCollection<DiaryMonth>();

        /// <summary>
        /// Most used words of diary logs
        /// </summary>
        public ObservableCollection<WordItem> WordList { get; set; }

        /// <summary>
        /// Black list for most used words
        /// </summary>
        public ObservableCollection<WordItem> BlackWordsList { get; set; }

        #region Stat Properties

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public List<string> Labels { get; set; } = new List<string>();
        public Func<double, string> YFormatter { get; set; }

        #endregion

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
            var months = new ObservableCollection<DiaryMonth>();

            for (int i = 1; i < 13; i++)
            {
                var days = new List<DiaryDay>();

                var firstDayOfMonth = new DateTime(TargetYear.YearNumber, i, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var lastDayLastMonth = firstDayOfMonth.AddDays(-1);

                var previousCoundown = PreviousNextMonthDays.GetPreviousCountdown(firstDayOfMonth.DayOfWeek);
                var nextCoundown = PreviousNextMonthDays.GetNextCountdown(lastDayOfMonth.DayOfWeek);

                // previous month days
                for (int ii = previousCoundown - 1; ii > -1; ii--)
                {
                    days.Add(new DiaryDay 
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

                    days.Add(new DiaryDay
                    {
                        IsEnabled = true,
                        IsDefault = date != DateTime.Now.Date ? false : true,
                        Date = date,
                        DayStatus = date != DateTime.Now.Date ? DayStatus.EmptyDay : DayStatus.EmptyToday
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
                    days.Add(new DiaryDay
                    {
                        Date = new DateTime(TargetYear.YearNumber, i + 1 > 12 ? 12 : i + 1, ii),
                        DayStatus = DayStatus.DayOfNextMonth
                    });
                }

                months.Add(new DiaryMonth 
                { 
                    MonthNumber = i,
                    MonthName = Settings.CultureInfo.DateTimeFormat.GetMonthName(i),
                    Year = TargetYear.YearNumber,
                    DiaryDays = days
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
            var diaryLogs = db.DiaryLogs
            .Where(x => EF.Functions.Like(x.Date.Year.ToString(), TargetYear.YearNumber.ToString()))
            .Select(x => new DiaryLog
            {
                Id = x.Id,
                Date = x.Date,
                LogContent = x.LogContent,
                AddedDate = x.AddedDate,
                UpdateDate = x.UpdateDate,
                LogContentLength = x.LogContentLength,
                DiaryLogRate = db.DiaryLogRates.FirstOrDefault(c => c.DiaryLogId == x.Id)
            }).ToList();

            // set diarylogs of days
            foreach (DiaryLog log in diaryLogs)
            {
                var targetMonth = log.Date.Month - 1;

                if (Months[targetMonth].DiaryDays.Any(x => x.Date == log.Date))
                {
                    var day = Months[targetMonth].DiaryDays.First(x => x.Date == log.Date);
                    day.DiaryLog = log;
                    day.DayStatus = log.Date.Date == DateTime.Now.Date ? DayStatus.LoggedToday : DayStatus.LoggedDay;
                }
            }

            // load black list
            BlackWordsList = db.DiaryBlackWords.Select(x => new WordItem
            {
                Id = x.Id,
                Word = x.Word,
                Count = x.Word.Length,
                Command = new RelayParameterizedCommand(RemoveFromBlackList)
            }).ToObservableCollection();

            // load most used words of diarylogs
            var wordList = diaryLogs.SelectMany(x => x.LogContent
            .Replace("\n", null).Replace("\r", null).ToLower().Split(new[] { ' ', '.', ',', '\'' }))
            .GroupBy(x => x)
            .Select(x => new WordItem
            {
                Word = Settings.CultureInfo.TextInfo.ToTitleCase(x.Key),
                Count = x.Count(),
                Command = new RelayParameterizedCommand(AddToBlackList)
            })
            .Where(x => x.Word.Length > 2 && x.Count > 10)
            .OrderBy(a => Guid.NewGuid())
            .ToList();

            // remove black list items from most used words list
            for (int i = 0; i < wordList.Count; i++)
            {
                if (BlackWordsList.Any(x => x.Word == wordList[i].Word.ToLower()))
                {
                    wordList.Remove(wordList[i]);
                }
            }

            // set filtered list to ToObservableCollection
            WordList = wordList.Take(200).ToObservableCollection();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Calendar button event for add or view log
        /// </summary>
        /// <param name="sender"></param>
        public void CalendarButtonAction(object sender)
        {
            var diaryDay = ((Button)sender).DataContext as DiaryDay;

            var dialog = new DiaryLogDialog();

            dialog.Closing += (s, a) =>
            {
                if (dialog.DataContext is AddDiaryLogViewModel vm && vm.DiaryLog.LogContentLength > 0)
                {
                    diaryDay.DayStatus = diaryDay.Date == DateTime.Now ? DayStatus.LoggedToday : DayStatus.LoggedDay;
                    diaryDay.DiaryLog = vm.DiaryLog;

                    for (int i = 0; i < Months.Count; i++)
                    {
                        if (Months[i].MonthNumber == diaryDay.Date.Month)
                        {
                            for (int ii = 0; ii < Months[i].DiaryDays.Count; ii++)
                            {
                                if (Months[i].DiaryDays[ii].Date == diaryDay.Date)
                                    Months[i].DiaryDays[ii] = diaryDay;
                                return;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new AddDiaryLogViewModel(dialog, diaryDay.Date, diaryDay.DiaryLog));
        }

        /// <summary>
        /// Word add to black list
        /// </summary>
        /// <param name="sender"></param>
        public void AddToBlackList(object sender)
        {
            var textBlock = sender as TextBlock;
            var wordItem = textBlock.DataContext as WordItem;

            var blackWord = new DiaryBlackWord
            {
                Word = wordItem.Word.ToLower()
            };

            using var db = new AppDbContext();

            if (!db.DiaryBlackWords.Any(x => x.Word == wordItem.Word.ToLower()))
            {
                db.DiaryBlackWords.Add(blackWord);

                db.SaveChanges();
            }

            WordList.Remove(wordItem);

            BlackWordsList.Add(new WordItem 
            {
                Id = blackWord.Id,
                Word = wordItem.Word,
                Command = new RelayParameterizedCommand(RemoveFromBlackList)
            });
        }

        /// <summary>
        /// Word remove from black list
        /// </summary>
        /// <param name="sender"></param>
        public void RemoveFromBlackList(object sender)
        {
            var textBlock = sender as TextBlock;
            var wordItem = textBlock.DataContext as WordItem;

            using var db = new AppDbContext();

            db.DiaryBlackWords.Remove(new DiaryBlackWord 
            {
                Id = wordItem.Id,
                Word = wordItem.Word
            });

            db.SaveChanges();

            var item = BlackWordsList.First(x => x.Word == wordItem.Word);

            BlackWordsList.Remove(item);

            WordList.Add(new WordItem 
            { 
                Id = wordItem.Id,
                Word = wordItem.Word,
                Count = 0,
                Command = new RelayParameterizedCommand(AddToBlackList)
            });
        }

        #endregion
    }
}