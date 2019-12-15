using Process.Models.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Diet
{
    public class DietDay : Day, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DietLogItem> DietLogItems { get; set; } = new ObservableCollection<DietLogItem>();
    }
}
