using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Process.Models.Workout;
using LiveCharts;
using LiveCharts.Wpf;

namespace Process.ViewModel.Workout
{
    class WorkoutTargetLogGraphViewModel : WindowViewModel
    {
        public WorkoutTargetLogGraphViewModel(Window window, WorkoutPlanItem workoutPlanItem) : base(window)
        {
            Title = $"Workout - Log Graph: {workoutPlanItem.WorkoutPlan.Title}";

            WorkoutPlanItem = workoutPlanItem;

            ApplyGraph();
        }

        #region Properties

        public WorkoutPlanItem WorkoutPlanItem { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public Func<double, string> Formatter { get; set; }

        #endregion

        #region Methods

        public void ApplyGraph()
        {

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Logs",
                    Values = new ChartValues<int>(),
                    Fill = System.Windows.Application.Current.FindResource("LightGreenBrush") as Brush
                },
                new ColumnSeries
                {
                    Title = "Targets",
                    Values = new ChartValues<int>(),
                    Fill = System.Windows.Application.Current.FindResource("SoftBlueBrush") as Brush
                }
            };

            foreach (var workoutDayItem in WorkoutPlanItem.WorkoutDayItems)
            {
                SeriesCollection[0].Values.Add(workoutDayItem.WorkoutDayCompleteCount);
                SeriesCollection[1].Values.Add(workoutDayItem.WorkoutTargetItems.Count);

                Labels.Add($"{workoutDayItem.WorkoutDay.Date} (#{workoutDayItem.WorkoutDay.DayNumber})");
            }
        }

        #endregion
    }
}