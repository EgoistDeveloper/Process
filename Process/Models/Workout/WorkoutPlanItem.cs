using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Workout
{
    public class WorkoutPlanItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WorkoutPlan WorkoutPlan { get; set; } = new WorkoutPlan();
        public bool IsExpired { get; set; }
        public int WorkoutPlanCompleteCount { get; set; }

        public ObservableCollection<WorkoutDayItem> WorkoutDayItems { get; set; } = new ObservableCollection<WorkoutDayItem>();

        public WorkoutResultItem WorkoutResultItem { get; set; }
    }
}
