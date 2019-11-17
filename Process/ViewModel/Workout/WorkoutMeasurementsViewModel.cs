using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Helpers;
using Process.Models.Workout;

namespace Process.ViewModel.Workout
{
    public class WorkoutMeasurementsViewModel : WindowViewModel
    {
        public WorkoutMeasurementsViewModel(Window window, WorkoutResultItem workoutResultItem) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = $"Save Workout Plan Results:";

            WorkoutResultItem = workoutResultItem;

            BodyPartItems = Enum.GetValues(typeof(BodyPart)).Cast<BodyPart>()
            .Select(x => new BodyPartItem()
            {
                BodyPartName = x.ToString(),
                BodyPart = x
            }).ToList().ToObservableCollection();

            foreach (var workoutMeasurement in WorkoutResultItem.WorkoutMeasurements)
            {
                BodyPartItems.Remove(BodyPartItems.First(x => x.BodyPart == workoutMeasurement.BodyPart));
            }

            SelectedBodyPartItem = BodyPartItems.FirstOrDefault();

            AddMeasurementCommand = new RelayCommand((p) => AddMeasurement());
            RemoveMeasurementCommand = new RelayParameterizedCommand(RemoveMeasurement);
        }

        #region Commands

        public ICommand AddMeasurementCommand { get; set; }
        public ICommand RemoveMeasurementCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<BodyPartItem> BodyPartItems { get; set; }
        public BodyPartItem SelectedBodyPartItem { get; set; }
        public WorkoutResultItem WorkoutResultItem { get; set; }
        public double Measurement { get; set; }

        #endregion

        #region Methods


        #endregion

        #region Command Methods

        public void AddMeasurement()
        {
            if (Measurement <= 0) return;

            var measurement = new WorkoutMeasurement
            {
                BodyPart = SelectedBodyPartItem.BodyPart,
                WorkoutResultId = WorkoutResultItem.WorkoutResult.Id,
                Measurement = Measurement
            };

            using var db = new AppDbContext();
            db.WorkoutMeasurements.Add(measurement);
            db.SaveChanges();

            WorkoutResultItem.WorkoutMeasurements.Add(measurement);
            BodyPartItems.Remove(SelectedBodyPartItem);
            Measurement = 0;
        }

        public void RemoveMeasurement(object sender)
        {
            var workoutMeasurement = (sender as Button).DataContext as WorkoutMeasurement;

            using var db = new AppDbContext();
            db.WorkoutMeasurements.Remove(workoutMeasurement);
            db.SaveChanges();

            var bodyPartItem = new BodyPartItem
            {
                BodyPart = workoutMeasurement.BodyPart,
                BodyPartName = workoutMeasurement.BodyPart.ToString()
            };

            BodyPartItems.Insert(0, bodyPartItem);
            SelectedBodyPartItem = bodyPartItem;

            WorkoutResultItem.WorkoutMeasurements.Remove(workoutMeasurement);
        }

        #endregion
    }
}