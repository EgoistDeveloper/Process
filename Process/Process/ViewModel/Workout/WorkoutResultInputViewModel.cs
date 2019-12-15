using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Dialogs.Workout;
using Process.Models.Workout;

namespace Process.ViewModel.Workout
{
    public class WorkoutResultInputViewModel : WindowViewModel
    {
        public WorkoutResultInputViewModel(Window window, WorkoutResultItem workoutResultItem = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 550;
            WindowMinimumWidth = 700;
            Title = $"Save Workout Plan Results";

            WorkoutResultItem = workoutResultItem;

            CloseCommand = new RelayCommand(p =>
            {
                SaveResult();

                mWindow.Close();
            });

            AddMeasurementCommand = new RelayParameterizedCommand(AddMeasurement);
            RemoveMeasurementCommand = new RelayParameterizedCommand(RemoveMeasurement);

            SaveResult();
        }

        #region Commands

        public ICommand AddMeasurementCommand { get; set; }
        public ICommand RemoveMeasurementCommand { get; set; }

        #endregion

        #region Properties

        public WorkoutResultItem WorkoutResultItem { get; set; }

        #endregion

        #region Methods

        public void SaveResult()
        {
            using var db = new AppDbContext();

            if (WorkoutResultItem.WorkoutResult != null && WorkoutResultItem.WorkoutResult.Id > 0)
            {
                db.WorkoutResults.Update(WorkoutResultItem.WorkoutResult);
            }
            else if (WorkoutResultItem.WorkoutResult != null)
            {
                WorkoutResultItem.WorkoutResult.AddedDate = DateTime.Now;

                db.WorkoutResults.Add(WorkoutResultItem.WorkoutResult);
            }

            db.SaveChanges();
        }

        #endregion

        #region Command Methods

        public void AddMeasurement(object sender)
        {
            var dialog = new WorkoutMeasurements();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is WorkoutMeasurementsViewModel vm)
                {
                    WorkoutResultItem = vm.WorkoutResultItem;
                }
            };

            dialog.ShowDialogWindow(new WorkoutMeasurementsViewModel(dialog, WorkoutResultItem), mWindow); ;
        }

        public void RemoveMeasurement(object sender)
        {
            var workoutMeasurement = (sender as Button).DataContext as WorkoutMeasurement;

            using var db = new AppDbContext();
            db.WorkoutMeasurements.Remove(workoutMeasurement);
            db.SaveChanges();

            WorkoutResultItem.WorkoutMeasurements.Remove(workoutMeasurement);
        }

        #endregion
    }
}