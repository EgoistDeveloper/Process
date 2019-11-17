using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Models.Workout;
using static Process.Helpers.ObservableCollectionHelper;

namespace Process.ViewModel.Workout
{
    public class WorkoutInputViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="toDoList">ToDo List Id</param>
        public WorkoutInputViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = "Add Workout";

            BodyPartItems = Enum.GetValues(typeof(BodyPart)).Cast<BodyPart>()
            .Select(x => new BodyPartItem()
            {
                BodyPartName = x.ToString(),
                BodyPart = x
            }).ToList();

            AddWorkoutCommand = new RelayCommand(p => AddWorkout());
            DeleteWorkoutCommand = new RelayParameterizedCommand(DeleteWorkout);

            LoadWorkouts();
        }

        #region Commands

        public ICommand AddWorkoutCommand { get; set; }
        public ICommand DeleteWorkoutCommand { get; set; }

        #endregion

        #region Properties

        public string WorkoutTitle { get; set; }
        public string WorkoutDescription { get; set; }
        public ObservableCollection<Models.Workout.Workout> Workouts { get; set; } = new ObservableCollection<Models.Workout.Workout>();
        public List<BodyPartItem> BodyPartItems { get; set; }
        public BodyPartItem SelectedBodyPartItem { get; set; }

        #endregion

        #region Methods

        public void LoadWorkouts()
        {
            using var db = new AppDbContext();
            Workouts = db.Workouts.ToObservableCollection();
        }

        #endregion

        #region Command Methods

        public void AddWorkout()
        {
            if (SelectedBodyPartItem == null || string.IsNullOrEmpty(WorkoutTitle)) return;

            var workout = new Models.Workout.Workout()
            {
                AddedDate = DateTime.Now,
                TargetBodyPart = SelectedBodyPartItem.BodyPart,
                WorkoutTitle = WorkoutTitle,
                WorkoutDescription = WorkoutDescription
            };

            using var db = new AppDbContext();
            db.Workouts.Add(workout);
            db.SaveChanges();

            Workouts.Insert(0, workout);
            WorkoutTitle = "";
            WorkoutDescription = "";
        }

        public void DeleteWorkout(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is Models.Workout.Workout workout)) return;

            using var db = new AppDbContext();
            db.Workouts.Remove(workout);
            db.SaveChanges();

            Workouts.Remove(workout);
        }

        #endregion
    }
}