using System.ComponentModel;

namespace Process.UI.Controls.Common.Calendar.Models
{
    public class Day : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public System.DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
        public string TooltipText { get; set; }
        public DayStatus DayStatus { get; set; }
        public object DayItem { get; set; }
    }
}