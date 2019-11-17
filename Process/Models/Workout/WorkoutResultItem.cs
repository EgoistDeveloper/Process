using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Workout
{
    public class WorkoutResultItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WorkoutResult WorkoutResult { get; set; }
        public virtual ObservableCollection<WorkoutMeasurement> WorkoutMeasurements { get; set; } = new ObservableCollection<WorkoutMeasurement>();
    }
}
