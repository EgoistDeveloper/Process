using Process.Models.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Diet
{
    public class DietLogMonth : Month, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DietDay> DietDays { get; set; } = new ObservableCollection<DietDay>();
    }
}
