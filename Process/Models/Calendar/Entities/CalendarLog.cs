using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Calendar
{
    public class CalendarLog : INotifyPropertyChanged
    {
        public CalendarLog()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public bool IsDone { get; set; }
        public string Description { get; set; }
    }
}