using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Workout
{
    public class WorkoutDayItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WorkoutDay WorkoutDay { get; set; }
        public int WorkoutDayCompleteCount { get; set; }

        public ObservableCollection<WorkoutTargetItem> WorkoutTargetItems { get; set; } = new ObservableCollection<WorkoutTargetItem>();
    }
}
