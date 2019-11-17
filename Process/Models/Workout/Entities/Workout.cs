using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class Workout : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public BodyPart TargetBodyPart { get; set; }
        [Required]
        public string WorkoutTitle { get; set; }
        public string WorkoutDescription { get; set; }
    }
}
