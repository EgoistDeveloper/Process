using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Models.Workout;

namespace Process.ViewModel.Workout
{
    public class WorkoutTargetInputViewModel : WindowViewModel
    {
        public WorkoutTargetInputViewModel(Window window, WorkoutPlanItem workoutPlanItem) : base(window)
        {
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = $"Add Workout Targets: {workoutPlanItem.WorkoutPlan.Title}";

            WorkoutPlanItem = workoutPlanItem;
            WorkoutTargetItems = workoutPlanItem.WorkoutDayItems[0] != null 
                ? workoutPlanItem.WorkoutDayItems[0].WorkoutTargetItems 
                : new ObservableCollection<WorkoutTargetItem>();

            AddWorkoutTargetCommand = new RelayCommand(p => AddWorkoutTarget());
            DeleteWorkoutTargetCommand = new RelayParameterizedCommand(DeleteWorkoutTarget);

            LoadWorkouts();
        }

        #region Commands

        public ICommand AddWorkoutTargetCommand { get; set; }
        public ICommand DeleteWorkoutTargetCommand { get; set; }

        #endregion

        #region Properties

        public int RequiredSets { get; set; }
        public int RequiredRepeats { get; set; }

        public WorkoutPlanItem WorkoutPlanItem { get; set; }

        public List<Models.Workout.Workout> Workouts { get; set; } = new List<Models.Workout.Workout>();
        public Models.Workout.Workout SelectedWorkout { get; set; }

        public ObservableCollection<WorkoutTargetItem> WorkoutTargetItems { get; set; }

        public int TargetCount { get; set; }

        #endregion

        #region Methods

        public void LoadWorkouts()
        {
            using var db = new AppDbContext();
            Workouts = db.Workouts.ToList();
        }

        #region Command Methods

        #endregion
        public void AddWorkoutTarget()
        {
            if (SelectedWorkout == null || RequiredSets == 0 || RequiredRepeats == 0) return;

            var workoutTarget = new WorkoutTarget()
            {
                AddedDate = DateTime.Now,
                WorkoutId = SelectedWorkout.Id,
                WorkoutPlanId = WorkoutPlanItem.WorkoutPlan.Id,
                RequiredSets = RequiredSets,
                RequiredRepeats = RequiredRepeats
            };

            using var db = new AppDbContext();
            db.WorkoutTargets.Add(workoutTarget);
            db.SaveChanges();

            TargetCount += 1;
            WorkoutTargetItems.Add(new WorkoutTargetItem()
            {
                Workout = SelectedWorkout,
                WorkoutTarget = workoutTarget
            });

            RequiredSets = 0;
            RequiredRepeats = 0;
        }

        public void DeleteWorkoutTarget(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is WorkoutTargetItem workoutTargetItem)) return;

            using var db = new AppDbContext();
            db.WorkoutTargets.Remove(workoutTargetItem.WorkoutTarget);
            db.SaveChanges();

            WorkoutTargetItems.Remove(workoutTargetItem);
            TargetCount -= 1;
        }

        #endregion
    }
}