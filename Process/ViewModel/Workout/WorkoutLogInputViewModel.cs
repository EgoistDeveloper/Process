using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Dialogs.Workout;
using Process.Models.Workout;

namespace Process.ViewModel.Workout
{
    public class WorkoutLogInputViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="workoutDay">Workout Day Item</param>
        public WorkoutLogInputViewModel(Window window, WorkoutDayItem workoutDayItem) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;

            Title = $"Workout Log - Day: {workoutDayItem.WorkoutDay.Date.ToShortDateString()}";

            WorkoutDayItem = SetTargetIndex(workoutDayItem);

            SetIsWorkoutDoneCommand = new RelayParameterizedCommand(SetIsWorkoutDone);
            SetAllWorkoutsDoneCommand = new RelayCommand(p => SetAllWorkoutsDone());
        }

        #region Commands


        public ICommand SetIsWorkoutDoneCommand { get; set; }
        public ICommand SetAllWorkoutsDoneCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Workout day item
        /// </summary>
        public WorkoutDayItem WorkoutDayItem { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set day is completed
        /// </summary>
        public void SetDayIsDone()
        {
            using var db = new AppDbContext();

            if (WorkoutDayItem.WorkoutTargetItems.Count > 0
            && WorkoutDayItem.WorkoutTargetItems.Count == WorkoutDayItem.WorkoutDayCompleteCount)
            {
                WorkoutDayItem.WorkoutDay.IsCompleted = true;
                db.WorkoutDays.Update(WorkoutDayItem.WorkoutDay);
            }
            else if (WorkoutDayItem.WorkoutDay.IsCompleted)
            {
                WorkoutDayItem.WorkoutDay.IsCompleted = false;
                db.WorkoutDays.Update(WorkoutDayItem.WorkoutDay);
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Set workout day item index
        /// </summary>
        /// <param name="workoutDayItem"><WorkoutDayItem/param>
        /// <returns></returns>
        public WorkoutDayItem SetTargetIndex(WorkoutDayItem workoutDayItem)
        {
            if (workoutDayItem.WorkoutTargetItems.Count > 0)
            {
                for (int i = 0; i < workoutDayItem.WorkoutTargetItems.Count; i++)
                {
                    workoutDayItem.WorkoutTargetItems[i].Index = (i + 1).ToString();
                }
            }

            return workoutDayItem;
        }

        /// <summary>
        /// Show workout graph of day
        /// </summary>
        public void ShowDayWorkoutGraph()
        {
            var dialog = new WorkoutDayGraphDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is WorkoutPlanInputViewModel)
                {
                    mWindow.Close();
                }
            };

            dialog.ShowDialogWindow(new WorkoutDayGraphViewModel(dialog, WorkoutDayItem), mWindow);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Set selected workout done
        /// </summary>
        /// <param name="sender">Workout CheckBox</param>
        public void SetIsWorkoutDone(object sender)
        {
            if (sender == null || !(sender is CheckBox checkBox)) return;
            if (!(checkBox.DataContext is WorkoutTargetItem workoutTargetItem)) return;

            using var db = new AppDbContext();

            // add or update wourkout
            _ = workoutTargetItem.WorkoutLog.Id > 0 ?
            db.WorkoutLogs.Update(workoutTargetItem.WorkoutLog) :
            db.WorkoutLogs.Add(workoutTargetItem.WorkoutLog);
            db.SaveChanges();

            // day completed
            WorkoutDayItem.WorkoutDayCompleteCount = checkBox.IsChecked == true 
                ? WorkoutDayItem.WorkoutDayCompleteCount + 1
                : WorkoutDayItem.WorkoutDayCompleteCount - 1;

            SetDayIsDone();
        }

        /// <summary>
        /// Set all workouts done
        /// </summary>
        public void SetAllWorkoutsDone()
        {
            foreach (var workoutTargetItem in WorkoutDayItem.WorkoutTargetItems)
            {
                workoutTargetItem.WorkoutLog.Sets = workoutTargetItem.WorkoutTarget.RequiredSets;
                workoutTargetItem.WorkoutLog.Repeats = workoutTargetItem.WorkoutTarget.RequiredRepeats;
                workoutTargetItem.WorkoutLog.IsCompleted = true;

                using var db = new AppDbContext();

                _ = workoutTargetItem.WorkoutLog.Id > 0 ? 
                db.WorkoutLogs.Update(workoutTargetItem.WorkoutLog) : 
                db.WorkoutLogs.Add(workoutTargetItem.WorkoutLog);
                db.SaveChanges();

                WorkoutDayItem.WorkoutDayCompleteCount = WorkoutDayItem.WorkoutTargetItems.Count;
                SetDayIsDone();
            }
        }

        #endregion
    }
}