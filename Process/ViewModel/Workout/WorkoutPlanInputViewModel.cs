using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Process.Data;
using Process.Models.Workout;

namespace Process.ViewModel.Workout
{
    public class WorkoutPlanInputViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Current window</param>
        public WorkoutPlanInputViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 350;
            WindowMinimumWidth = 600;
            Title = "Add New Plan";

            CloseCommand = new RelayCommand(p =>
            {
                SavePlan();

                mWindow.Close();
            });
        }

        #region Properties

        public DateTime WorkoutPlanExpireDate { get; set; } = DateTime.Today.AddDays(7).Date;

        public WorkoutPlanItem WorkoutPlanItem { get; set; } = new WorkoutPlanItem();

        #endregion

        #region Methods

        public void SavePlan()
        {
            if (string.IsNullOrWhiteSpace(WorkoutPlanItem.WorkoutPlan.Title) 
                || WorkoutPlanExpireDate == null) return;

            WorkoutPlanItem.WorkoutPlan.AddedDate = DateTime.Now;
            WorkoutPlanItem.WorkoutPlan.ExpireDate = WorkoutPlanExpireDate;

            using var db = new AppDbContext();
            db.WorkoutPlans.Add(WorkoutPlanItem.WorkoutPlan);
            db.SaveChanges();

            var dayNumber = 1;

            foreach (var date in DateRange(DateTime.Now, WorkoutPlanExpireDate))
            {
                var workoutDay = new WorkoutDay()
                {
                    WorkoutPlanId = WorkoutPlanItem.WorkoutPlan.Id,
                    Date = date,
                    DayNumber = dayNumber,
                    IsCompleted = false
                };

                db.WorkoutDays.Add(workoutDay);
                db.SaveChanges();

                WorkoutPlanItem.WorkoutDayItems.Add(new WorkoutDayItem
                {
                    WorkoutDay = workoutDay
                });

                dayNumber += 1;
            }
        }

        private IEnumerable<DateTime> DateRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        #endregion
    }
}