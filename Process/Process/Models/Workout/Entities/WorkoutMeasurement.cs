using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class WorkoutMeasurement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long WorkoutResultId { get; set; }
        [Required]
        public BodyPart BodyPart { get; set; }
        [Required]
        public double Measurement { get; set; }
    }
}
