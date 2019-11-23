using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

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
        public BitmapImage Image { get; set; }
        public string VideoLink { get; set; }
    }
}