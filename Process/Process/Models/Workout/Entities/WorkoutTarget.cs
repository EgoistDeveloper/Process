using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class WorkoutTarget : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long WorkoutId { get; set; }
        [Required]
        public long WorkoutPlanId { get; set; }
        [Required]
        public int RequiredSets { get; set; }
        [Required]
        public int RequiredRepeats { get; set; }
    }
}
