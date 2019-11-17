using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs;
using Process.Dialogs.Workout;
using Process.Models.Workout;
using Process.ViewModel.App;
using static Process.Helpers.ObservableCollectionHelper;

namespace Process.ViewModel.Workout
{
    public class WorkoutLogViewModel : ViewModelBase
    {
        public WorkoutLogViewModel()
        {
            AddWorkoutCommand = new RelayCommand(p => AddWorkout());
            AddWorkoutPlanCommand = new RelayCommand(p => AddWorkoutPlan());
            WorkoutPlanDeleteCommand = new RelayParameterizedCommand(WorkoutPlanDelete);
            AddWorkoutTargetsCommand = new RelayParameterizedCommand(AddWorkoutTargets);
            AddWorkoutLogCommand = new RelayParameterizedCommand(AddWorkoutLog);
            ShowTargetLogGraphCommand = new RelayParameterizedCommand(ShowTargetLogGraph);
            ShowMeasurementsCommand = new RelayParameterizedCommand(ShowMeasurements);
            SetIsBreakCommand = new RelayParameterizedCommand(SetIsBreak);

            LoadWorkoutPlans();
        }

        #region Commands

        public ICommand AddWorkoutCommand { get; set; }
        public ICommand AddWorkoutPlanCommand { get; set; }
        public ICommand WorkoutPlanDeleteCommand { get; set; }
        public ICommand AddWorkoutTargetsCommand { get; set; }
        public ICommand AddWorkoutLogCommand { get; set; }
        public ICommand ShowTargetLogGraphCommand { get; set; }
        public ICommand ShowMeasurementsCommand { get; set; }
        public ICommand SetIsBreakCommand { get; set; }

        #endregion

        #region Public Properties

        public ObservableCollection<WorkoutPlanItem> WorkoutPlanItems { get; set; } = new ObservableCollection<WorkoutPlanItem>();

        #endregion

        #region Methods

        /// <summary>
        /// Load all workout plans
        /// </summary>
        private void LoadWorkoutPlans()
        {
            using var db = new AppDbContext();
            WorkoutPlanItems = db.WorkoutPlans.Select(wPlan => new WorkoutPlanItem
            {
                WorkoutPlan = wPlan,
                IsExpired = wPlan.ExpireDate < DateTime.Now,

                WorkoutResultItem = db.WorkoutResults.Where(wResult => wResult.WorkoutPlanId == wPlan.Id)
                .Select(wResult => new WorkoutResultItem
                {
                    WorkoutResult = wResult,
                    WorkoutMeasurements = db.WorkoutMeasurements
                    .Where(wMeasurement => wMeasurement.WorkoutResultId == wResult.Id)
                    .ToObservableCollection()
                })
                .FirstOrDefault()
                ?? new WorkoutResultItem
                {
                    WorkoutResult = new WorkoutResult
                    {
                        WorkoutPlanId = wPlan.Id
                    }
                },

                WorkoutDayItems = db.WorkoutDays.Where(wDay => wDay.WorkoutPlanId == wPlan.Id)
                .Select(wDay => new WorkoutDayItem
                {
                    WorkoutDay = wDay,

                    WorkoutTargetItems = db.WorkoutTargets.Where(wTarget => wTarget.WorkoutPlanId == wPlan.Id)
                    .Select(wTarget => new WorkoutTargetItem
                    {
                        WorkoutTarget = wTarget,
                        Workout = db.Workouts.First(x => x.Id == wTarget.WorkoutId) ?? new Models.Workout.Workout(),
                        WorkoutLog = db.WorkoutLogs.FirstOrDefault(x => x.WorkoutDayId == wDay.Id && x.WorkoutTargetId == wTarget.Id)
                        ?? new WorkoutLog
                        {
                            AddedDate = DateTime.Now,
                            WorkoutDayId = wDay.Id,
                            WorkoutTargetId = wTarget.Id,
                            WorkoutId = wTarget.WorkoutId
                        }
                    })
                    .ToObservableCollection(),

                WorkoutDayCompleteCount = db.WorkoutLogs.Count(x => x.WorkoutDayId == wDay.Id)
            }).ToObservableCollection(),
                WorkoutPlanCompleteCount = db.WorkoutDays.Count(x => x.WorkoutPlanId == wPlan.Id && x.IsCompleted)
            })
            .OrderByDescending(wPlan => wPlan.WorkoutPlan.Id)
            .ToObservableCollection();
        }

        /// <summary>
        /// Reload workout plan item
        /// </summary>
        /// <param name="workoutPlanId"></param>
        /// <returns></returns>
        private WorkoutPlanItem LoadWorkoutPlanItem(long workoutPlanId)
        {
            using var db = new AppDbContext();

            return db.WorkoutPlans.Where(wPlan => wPlan.Id == workoutPlanId).Select(wPlan => new WorkoutPlanItem
            {
                WorkoutPlan = wPlan,
                IsExpired = wPlan.ExpireDate < DateTime.Now,

                WorkoutDayItems = db.WorkoutDays.Where(wDay => wDay.WorkoutPlanId == wPlan.Id)
                    .Select(wDay => new WorkoutDayItem
                    {
                        WorkoutDay = wDay,

                        WorkoutTargetItems = db.WorkoutTargets.Where(wTarget => wTarget.WorkoutPlanId == wPlan.Id)
                        .Select(wTarget => new WorkoutTargetItem
                        {
                            WorkoutTarget = wTarget,
                            Workout = db.Workouts.First(x => x.Id == wTarget.WorkoutId) ?? new Models.Workout.Workout(),
                            WorkoutLog = db.WorkoutLogs.FirstOrDefault(x => x.WorkoutDayId == wDay.Id && x.WorkoutTargetId == wTarget.Id)
                            ?? new WorkoutLog
                            {
                                AddedDate = DateTime.Now,
                                WorkoutDayId = wDay.Id,
                                WorkoutTargetId = wTarget.Id,
                                WorkoutId = wTarget.WorkoutId
                            }
                        }).ToObservableCollection(),

                        WorkoutDayCompleteCount = db.WorkoutLogs.Count(x => x.WorkoutDayId == wDay.Id)
                    }).ToObservableCollection(),
                WorkoutPlanCompleteCount = db.WorkoutDays.Count(x => x.IsCompleted)
            }).Single();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Add workout
        /// </summary>
        public void AddWorkout()
        {
            var dialog = new WorkoutInputDialog();

            dialog.ShowDialogWindow(new WorkoutInputViewModel(dialog));
        }

        /// <summary>
        /// Add new workout plan and days
        /// </summary>
        public void AddWorkoutPlan()
        {
            var dialog = new WorkoutPlanInputDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is WorkoutPlanInputViewModel vm && vm.WorkoutPlanItem.WorkoutPlan.Id > 0)
                {
                    WorkoutPlanItems.Insert(0, vm.WorkoutPlanItem);
                }
            };

            dialog.ShowDialogWindow(new WorkoutPlanInputViewModel(dialog));
        }

        /// <summary>
        /// Delete selected workout plan and items
        /// </summary>
        /// <param name="sender">Sender button</param>
        public void WorkoutPlanDelete(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is WorkoutPlanItem workoutPlanItem)) return;

            var dialog = new DeleteDialog();

            dialog.Closing += (send, args) =>
            {
                if (dialog.DataContext is DeleteDialogViewModel vm && vm.Result)
                {
                    using var db = new AppDbContext();
                    db.WorkoutPlans.Remove(workoutPlanItem.WorkoutPlan);
                    db.SaveChanges();

                    WorkoutPlanItems.Remove(workoutPlanItem);
                }
            };

            dialog.ShowDialogWindow(new DeleteDialogViewModel(dialog, "Delete Workout Plan", workoutPlanItem.WorkoutPlan.Title));
        }

        /// <summary>
        /// Add workout targets to plan (reloads plan and items)
        /// </summary>
        /// <param name="sender">Sender button</param>
        public void AddWorkoutTargets(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is WorkoutPlanItem workoutPlanItem)) return;

            var dialog = new WorkoutTargetInputDialog();

            dialog.Closing += (sen, args) =>
            {
                if (!(dialog.DataContext is WorkoutTargetInputViewModel vm) || vm.TargetCount <= 0) return;

                for (var i = 0; i < WorkoutPlanItems.Count; i++)
                {
                    if (WorkoutPlanItems[i].WorkoutPlan.Id == workoutPlanItem.WorkoutPlan.Id)
                    {
                        WorkoutPlanItems[i] = LoadWorkoutPlanItem(workoutPlanItem.WorkoutPlan.Id);
                    }
                }
            };

            dialog.ShowDialogWindow(new WorkoutTargetInputViewModel(dialog, workoutPlanItem)); ;
        }

        /// <summary>
        /// Add completed workout log
        /// </summary>
        /// <param name="sender">Sender button</param>
        public void AddWorkoutLog(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is WorkoutDayItem workoutDayItem)) return;

            var dialog = new WorkoutLogInputDialog();

            dialog.Closing += (s, e) =>
            {
                var workoutPlanItem = WorkoutPlanItems.Single(x => x.WorkoutPlan.Id == workoutDayItem.WorkoutDay.WorkoutPlanId);

                if (workoutPlanItem != null && workoutPlanItem.WorkoutDayItems != null && workoutPlanItem.WorkoutDayItems.Count > 0)
                {
                    workoutPlanItem.WorkoutPlanCompleteCount = workoutPlanItem.WorkoutDayItems.Count(x => x.WorkoutDay.IsCompleted);

                    if (workoutPlanItem.WorkoutPlanCompleteCount > 0 && workoutPlanItem.WorkoutPlanCompleteCount == workoutPlanItem.WorkoutDayItems.Count)
                    {
                        workoutPlanItem.WorkoutPlan.IsCompleted = true;

                        using var db = new AppDbContext();
                        db.WorkoutPlans.Update(workoutPlanItem.WorkoutPlan);
                        db.SaveChanges();
                    }
                }
            };

            dialog.ShowDialogWindow(new WorkoutLogInputViewModel(dialog, workoutDayItem));
        }

        /// <summary>
        /// Show target-log graph
        /// </summary>
        /// <param name="sender">Sender button</param>
        public void ShowTargetLogGraph(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is WorkoutPlanItem workoutPlanItem)) return;

            var dialog = new WorkoutTargetLogGraphDialog();
            dialog.ShowDialogWindow(new WorkoutTargetLogGraphViewModel(dialog, workoutPlanItem)); ;
        }

        /// <summary>
        /// Show measurement reults
        /// </summary>
        /// <param name="sender"></param>
        public void ShowMeasurements(object sender)
        {
            var workoutPlanItem = (sender as Button).DataContext as WorkoutPlanItem;

            var dialog = new WorkoutResultDialog();
            dialog.ShowDialogWindow(new WorkoutResultInputViewModel(dialog, workoutPlanItem.WorkoutResultItem)); ;
        }

        public void SetIsBreak(object sender)
        {
            var workoutDayItem = (sender as CheckBox).DataContext as WorkoutDayItem;

            using var db = new AppDbContext();
            db.WorkoutDays.Update(workoutDayItem.WorkoutDay);
            db.SaveChanges();
        }

        #endregion
    }
}