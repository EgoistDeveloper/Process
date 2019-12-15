using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Process.Models.Workout
{
    public class WorkoutDayItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands

        public ICommand ShowWorkoutDayCommand { get; set; }
        public ICommand SetAllDoneCommand { get; set; }
        public ICommand SetAllUndoneCommand { get; set; }

        #endregion

        public WorkoutDay WorkoutDay { get; set; }
        public int WorkoutDayCompleteCount { get; set; }

        public ObservableCollection<WorkoutTargetItem> WorkoutTargetItems { get; set; } = new ObservableCollection<WorkoutTargetItem>();

    }
}