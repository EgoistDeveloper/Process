using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class WorkoutDay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long WorkoutPlanId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int DayNumber { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsBreak { get; set; }

        public virtual ObservableCollection<WorkoutLog> WorkoutLogs { get; set; } = new ObservableCollection<WorkoutLog>();
    }
}
