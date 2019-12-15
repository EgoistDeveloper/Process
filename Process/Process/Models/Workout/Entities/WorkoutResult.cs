using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Process.Models.Workout
{
    public class WorkoutResult : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long WorkoutPlanId { get; set; }
        [Required]
        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double Weight3 { get; set; }
        public string WorkoutPlanNotes { get; set; }

        public virtual ObservableCollection<WorkoutMeasurement> WorkoutMeasurements { get; set; } = new ObservableCollection<WorkoutMeasurement>();
    }
}
