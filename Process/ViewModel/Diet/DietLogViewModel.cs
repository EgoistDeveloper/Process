using GalaSoft.MvvmLight;
using Process.Data;
using Process.Helpers;
using Process.Models.Diet;
using Process.Models.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Process.ViewModel.Diet
{
    public class DietLogViewModel : ViewModelBase
    {
        public DietLogViewModel()
        {
            SearchFoodCommand = new RelayParameterizedCommand(SearchFood);
            SelectDateCommand = new RelayParameterizedCommand(SelectDate);
            FoodChangedCommand = new RelayCommand(p => FoodChanged());
            AddDietLogCommand = new RelayCommand(p => AddDietLog());
            DayLogFoodChangedCommand = new RelayCommand(p => DayLogFoodChanged());
            LoadFoods();
            LoadMonths();
        }

        #region Commands

        public ICommand SearchFoodCommand { get; set; }
        public ICommand FoodChangedCommand { get; set; }
        public ICommand SelectDateCommand { get; set; }
        public ICommand AddDietLogCommand { get; set; }
        public ICommand DayLogFoodChangedCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<DietLogItem> DietLogItems { get; set; } = new ObservableCollection<DietLogItem>();
        public ObservableCollection<DietFood> DietFoods { get; set; } = new ObservableCollection<DietFood>();
        public ObservableCollection<DietFood> DietFoodsClone { get; set; } = new ObservableCollection<DietFood>();
        public DietFood SelectedDietFood { get; set; }
        public DietNutrientAndEnergyValue SelectedDietNutrientAndEnergyValue { get; set; }
        public bool IsFoodDropDownOpen { get; set; }
        public string FoodAmount { get; set; }
        public DietLogMonth DietLogMonth { get; set; }
        public DietDay SelectedDietDay { get; set; }
        public DietLogItem SelectedDietLogItem { get; set; }

        #endregion

        #region Methods

        public void LoadFoods()
        {
            using var db = new AppDbContext();
            var lastUsedFoods = db.DietLastSelectedFoods
            .Where(x => x.AddedDate.Month - DateTime.Now.Month > 3)
            .OrderByDescending(x => x.AddedDate)
            .Take(100).ToList();

            foreach (var lastUsedFood in lastUsedFoods)
            {
                DietFoods.Add(db.DietFoods.FirstOrDefault(x => x.Id == lastUsedFood.DietFoodId));
            }

            if (lastUsedFoods.Count() < 100)
            {
                var dietFoodsExtra = db.DietFoods.Take(100 - lastUsedFoods.Count()).ToList();

                if (dietFoodsExtra.Count() > 0)
                {
                    foreach (var dietFood in dietFoodsExtra)
                    {
                        DietFoods.Add(dietFood);
                    }
                }
            }

            DietFoodsClone = DietFoods;
        }

        public void LoadMonths()
        {
            var date = DateTime.Now;

            DietLogMonth = new DietLogMonth
            {
                MonthNumber = date.Month,
                MonthName = Settings.CultureInfo.DateTimeFormat.GetMonthName(date.Month),
                Year = date.Year
            };

            var DietDays = new List<DietDay>();

            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var lastDayLastMonth = firstDayOfMonth.AddDays(-1);

            var previousCoundown = PreviousNextMonthDays.GetPreviousCountdown(firstDayOfMonth.DayOfWeek);
            var nextCoundown = PreviousNextMonthDays.GetNextCountdown(lastDayOfMonth.DayOfWeek);

            // previous month days
            for (int ii = previousCoundown - 1; ii > -1; ii--)
            {
                DietDays.Add(new DietDay
                {
                    Date = new DateTime(date.Year, date.Month - 1, lastDayLastMonth.Day - ii),
                    DayStatus = DayStatus.DayOfPreviousMonth
                });
            }

            // this month days
            var thisMonthDayCount = DateTime.DaysInMonth(date.Year, date.Month);

            for (int ii = 1; ii < thisMonthDayCount + 1; ii++)
            {
                var dietDay = new DietDay
                {
                    Date = new DateTime(date.Year, date.Month, ii),
                    DayStatus = DayStatus.EmptyToday,
                    IsEnabled = true
                };

                using var db = new AppDbContext();
                if (db.DietLogs.Any(x => x.AddedDate.ToShortDateString() == dietDay.Date.ToShortDateString()))
                {
                    dietDay.DietLogItems = db.DietLogs.Where(x => x.AddedDate.ToShortDateString() == dietDay.Date.ToShortDateString()).Select(x => new DietLogItem
                    {
                        DietLog = x,
                        DietFood = db.DietFoods.FirstOrDefault(c => c.Id == x.DietFoodId)
                    }).ToObservableCollection();
                }

                if (date.Day == ii)
                {
                    dietDay.DayStatus = dietDay.DietLogItems.Count > 0 ? DayStatus.LoggedToday : DayStatus.EmptyToday;
                    dietDay.IsDefault = true;
                }
                else
                {
                    dietDay.DayStatus = dietDay.DietLogItems.Count > 0 ? DayStatus.LoggedDay : DayStatus.EmptyDay;
                }

                DietDays.Add(dietDay);
            }

            // next month days
            var daysBlockCount = previousCoundown + nextCoundown + thisMonthDayCount;

            if (daysBlockCount < 42)
            {
                nextCoundown += 42 - daysBlockCount;
            }

            for (int ii = 1; ii < nextCoundown + 1; ii++)
            {
                DietDays.Add(new DietDay
                {
                    Date = new DateTime(date.Year, date.Month + 1, ii),
                    DayStatus = DayStatus.DayOfNextMonth
                });
            }

            DietLogMonth.DietDays = DietDays.ToObservableCollection();
        }

        #endregion

        #region Command Methods

        public void SearchFood(object sender)
        {
            if (sender == null || !(sender is ComboBox)) return;
            var comboBox = sender as ComboBox;

            IsFoodDropDownOpen = true;

            if (!string.IsNullOrWhiteSpace(comboBox.Text) && comboBox.Text.Length > 2)
            {
                using var db = new AppDbContext();
                DietFoods = new ObservableCollection<DietFood>(
                db.DietFoods.Where(x => EF.Functions.Like(x.Title, $"%{comboBox.Text}%"))
                .Take(50).ToList());
            }
            else if (string.IsNullOrWhiteSpace(comboBox.Text))
            {
                DietFoods = DietFoodsClone;
            }
        }

        public void SelectDate(object sender)
        {
            if (sender == null || !(sender is Button)) return;
            var button = sender as Button;

            SelectedDietDay = button.DataContext as DietDay;

            for (int i = 0; i < DietLogMonth.DietDays.Count; i++)
            {
                if (DietLogMonth.DietDays[i] == SelectedDietDay)
                {
                    DietLogMonth.DietDays[i].IsDefault = true;
                    DietLogMonth.DietDays[i].DayStatus = DietLogMonth.DietDays[i].DietLogItems.Count > 0 ? DayStatus.LoggedToday : DayStatus.EmptyToday;
                }
                else
                {
                    DietLogMonth.DietDays[i].IsDefault = false;
                }
            }
        }

        public void FoodChanged()
        {
            if (SelectedDietFood != null && SelectedDietFood.Id > 0)
            {
                using var db = new AppDbContext();
                SelectedDietNutrientAndEnergyValue
                = db.DietNutrientAndEnergyValues.Where(x => x.DietFoodId == SelectedDietFood.Id)
                .Select(x => new DietNutrientAndEnergyValue
                {
                    Id = x.Id,
                    Calorie = x.Calorie,
                    CalorieDailyVal = x.CalorieDailyVal,
                    DietMineralValues = db.DietMineralValues.Where(c => c.DietNutrientAndEnergyValueId == x.Id)
                    .ToObservableCollection(),
                    DietVitaminValues = db.DietVitaminValues.Where(c => c.DietNutrientAndEnergyValueId == x.Id)
                    .ToObservableCollection(),
                    DietNutrientTypeValues = db.DietNutrientTypeValues.Where(c => c.DietNutrientAndEnergyValueId == x.Id)
                    .ToObservableCollection()
                }).Single();
            }
        }

        public void AddDietLog()
        {
            if (SelectedDietDay == null || string.IsNullOrWhiteSpace(FoodAmount) || !double.TryParse(FoodAmount, out double amount)) return;

            var dietLog = new DietLog
            {
                AddedDate = SelectedDietDay.Date,
                DietFoodId = SelectedDietFood.Id,
                Amount = amount
            };

            using var db = new AppDbContext();
            db.DietLogs.Add(dietLog);
            db.SaveChanges();

            SelectedDietDay.DietLogItems.Insert(0, new DietLogItem
            {
                DietLog = dietLog,
                DietFood = SelectedDietFood
            });

            FoodAmount = null;
            SelectedDietFood = null;
            SelectedDietNutrientAndEnergyValue = null;
        }

        public void DayLogFoodChanged()
        {
            SelectedDietFood = SelectedDietLogItem.DietFood;
            FoodChanged();
        }

        #endregion
    }
}
