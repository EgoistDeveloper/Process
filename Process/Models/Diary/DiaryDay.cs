using Process.Models.Common;
using System.ComponentModel;

namespace Process.Models.Diary
{
    public class DiaryDay : Day, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DiaryLog DiaryLog { get; set; }
    }
}
