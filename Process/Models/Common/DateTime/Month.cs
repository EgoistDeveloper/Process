using System.ComponentModel;


namespace Process.Models.Common
{
    public class Month : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int MonthNumber { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
    }
}
