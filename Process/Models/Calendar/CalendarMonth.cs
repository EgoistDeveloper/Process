using Process.Models.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace Process.Models.Calendar
{
    public class CalendarMonth : Month, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<CalendarDay> CalendarDays { get; set; }
    }
}
