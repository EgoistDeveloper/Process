using Process.Models.Common;
using System.ComponentModel;

namespace Process.Models.Calendar
{
    public class CalendarDay : Day, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarLog CalendarLog { get; set; } = new CalendarLog();
    }
}
