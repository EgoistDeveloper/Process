using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Workout
{
    public class WorkoutPlan : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        public string Description { get; set; }

        public WorkoutResult WorkoutResult { get; set; }
        public virtual ObservableCollection<WorkoutDay> WorkoutDays { get; set; } = new ObservableCollection<WorkoutDay>();
        public virtual ObservableCollection<WorkoutTarget> WorkoutTargets { get; set; } = new ObservableCollection<WorkoutTarget>();
    }
}
