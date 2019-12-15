using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class WorkoutLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long WorkoutId { get; set; }
        [Required]
        public long WorkoutDayId { get; set; }
        [Required]
        public long WorkoutTargetId { get; set; }
        [Required]
        public int Sets { get; set; }
        [Required]
        public int Repeats { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}
