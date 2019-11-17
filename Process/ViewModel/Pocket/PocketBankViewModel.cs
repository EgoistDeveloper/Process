using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs.Pocket;
using Process.Helpers;
using Process.Models.Pocket;
using Process.Models.Workout;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;

namespace Process.ViewModel.PocketBank
{
    public class PocketBankViewModel : ViewModelBase
    {
        public PocketBankViewModel()
        {
            AddInOutLogCommand = new RelayCommand(p => AddInOutLog());
            DeletePocketInOutCommand = new RelayParameterizedCommand(DeletePocketInOut);

            YFormatter = val => string.Format(CultureInfo.InvariantCulture, "{0:0.00}", val);


            LoadInOutItems();
        }

        public ObservableCollection<PocketInOutItem> PocketInOutItems { get; set; } = new ObservableCollection<PocketInOutItem>();
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public Func<double, string> YFormatter { get; set; }


        #region Commands

        public ICommand AddInOutLogCommand { get; set; }
        public ICommand DeletePocketInOutCommand { get; set; }

        #endregion

        #region Command Methods

        public void AddInOutLog()
        {
            var dialog = new InOutInputDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is InOutInputViewModel vm && vm.PocketInOut.Id > 0)
                {
                    PocketInOutItems.Insert(0, LoadInOutItem(vm.PocketInOut.Id));
                }
            };

            dialog.ShowDialogWindow(new InOutInputViewModel(dialog));
        }

        public void DeletePocketInOut(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is PocketInOutItem pocketInOutItem)) return;

            using var db = new AppDbContext();
            db.PocketInOuts.Remove(pocketInOutItem.PocketInOut);
            db.SaveChanges();

            PocketInOutItems.Remove(pocketInOutItem);
        }
        #endregion

        private void LoadInOutItems()
        {
            using var db = new AppDbContext();
            PocketInOutItems = db.PocketInOuts.Where(pInOut => pInOut.AddedDate.Year == DateTime.Now.Year).Select(pInOut => new PocketInOutItem
            {
                PocketInOut = pInOut,
                PocketInOutTypeItem = new PocketInOutTypeItem
                {
                    PocketInOutType = pInOut.Type,
                    PocketInOutTypeName = pInOut.Type.ToString()
                },
                PocketActionItem = db.PocketActions.Where(pAction => pAction.Id == pInOut.PocketActionId)
                .Select(pAction => new PocketActionItem
                {
                    PocketAction = pAction,
                    PocketCategory = db.PocketCategories.FirstOrDefault(pCategory => pCategory.Id == pAction.PocketCategoryId)
                }).Single()
                        })
            .OrderByDescending(pInOut => pInOut.PocketInOut.Id)
            .ToObservableCollection();


            LoadChartGraph();
        }

        private PocketInOutItem LoadInOutItem(long pocketInOutId)
        {
            PocketInOutItem pocketInOutItem;

            using var db = new AppDbContext();
            pocketInOutItem = db.PocketInOuts.Where(pInOut => pInOut.Id == pocketInOutId)
            .Select(pInOut => new PocketInOutItem
            {
                PocketInOut = pInOut,
                PocketInOutTypeItem = new PocketInOutTypeItem
                {
                    PocketInOutType = pInOut.Type,
                    PocketInOutTypeName = pInOut.Type.ToString()
                },
                PocketActionItem = db.PocketActions.Where(pAction => pAction.Id == pInOut.PocketActionId)
                 .Select(pAction => new PocketActionItem
                 {
                     PocketAction = pAction,
                     PocketCategory = db.PocketCategories.FirstOrDefault(pCategory => pCategory.Id == pAction.PocketCategoryId)
                 }).Single()
            }).Single();


            return pocketInOutItem;
        }

        public void LoadChartGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "In",
                    Values = new ChartValues<double>(),
                    Fill = System.Windows.Application.Current.FindResource("LightYellowBrush") as Brush
                },
                new ColumnSeries
                {
                    Title = "Out",
                    Values = new ChartValues<double>(),
                    Fill = System.Windows.Application.Current.FindResource("SoftBlueBrush") as Brush
                }
            };

            for (int i = 0; i < 12; i++)
            {
                SeriesCollection[0].Values.Add(0.0);
                SeriesCollection[1].Values.Add(0.0);

                Labels.Add(new DateTime(DateTime.Now.Year, i + 1, 1).ToString("MMMM", CultureInfo.InvariantCulture));
            }

            foreach (var pocketInOutItem in PocketInOutItems.GroupBy(x => x.PocketInOut.AddedDate.Month))
            {
                for (int i = 0; i < SeriesCollection[0].Values.Count; i++)
                {
                    if (i == pocketInOutItem.First().PocketInOut.AddedDate.Month - 1)
                    {
                        SeriesCollection[0].Values[i] = pocketInOutItem
                            .Where(x => x.PocketInOut.Type == PocketInOutType.In).Sum(x => x.PocketInOut.Amount);
                        SeriesCollection[1].Values[i] = pocketInOutItem
                            .Where(x => x.PocketInOut.Type == PocketInOutType.Out).Sum(x => x.PocketInOut.Amount);
                    }
                }
            }
        }
    }
}
