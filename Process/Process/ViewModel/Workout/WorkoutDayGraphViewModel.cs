using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Wpf;
using Process.Models.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Process.ViewModel.Workout
{
    public class WorkoutDayGraphViewModel : WindowViewModel
    {
        public WorkoutDayGraphViewModel(Window window, WorkoutDayItem workoutDayItem) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 650;
            WindowMinimumWidth = 900;

            Title = $"Workout Graph - Day: {workoutDayItem.WorkoutDay.Date.ToShortDateString()}";

            CloseCommand = new RelayCommand(p =>
            {
                mWindow.Close();
            });
            WorkoutDayItem = workoutDayItem;

            LoadGraph(WorkoutDayItem);
        }

        #region Properties

        /// <summary>
        /// Workout day item
        /// </summary>
        public WorkoutDayItem WorkoutDayItem { get; set; }

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public List<string> Labels { get; set; } = new List<string>();
        public Func<double, string> YFormatter { get; set; }

        #endregion

        #region Methods

        public void LoadGraph(WorkoutDayItem workoutDayItem)
        {
            if (workoutDayItem != null)
            {
                SeriesCollection = new SeriesCollection();

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Target Sets",
                        Values = new ChartValues<int> { },
                        PointGeometry = DefaultGeometries.Square
                    },
                    new LineSeries
                    {
                        Title = "Done Sets",
                        Values = new ChartValues<int> { },
                        PointGeometry = DefaultGeometries.Square
                    },
                    new LineSeries
                    {
                        Title = "Target Repeats",
                        Values = new ChartValues<int> { },
                        PointGeometry = DefaultGeometries.Diamond
                    },
                    new LineSeries
                    {
                        Title = "Done Repeats",
                        Values = new ChartValues<int> { },
                        PointGeometry = DefaultGeometries.Diamond
                    }
                };

                foreach (var workoutTargetItems in workoutDayItem.WorkoutTargetItems)
                {
                    // sets
                    SeriesCollection[0].Values.Add(workoutTargetItems.WorkoutTarget.RequiredSets);
                    SeriesCollection[1].Values.Add(workoutTargetItems.WorkoutLog.Sets);

                    // repeats
                    SeriesCollection[2].Values.Add(workoutTargetItems.WorkoutTarget.RequiredRepeats);
                    SeriesCollection[3].Values.Add(workoutTargetItems.WorkoutLog.Repeats);
                }
            }
        }

        #endregion
    }
}
