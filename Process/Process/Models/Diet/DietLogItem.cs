using System.ComponentModel;


namespace Process.Models.Diet
{
    public class DietLogItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DietLog DietLog { get; set; }
        public DietFood DietFood { get; set; }
    }
}
