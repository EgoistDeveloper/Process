using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Models.Workout
{
    public class WorkoutTargetItem
    {
        public WorkoutTarget WorkoutTarget { get; set; }
        public Workout Workout { get; set; }
        public WorkoutLog WorkoutLog { get; set; }
    }
}
