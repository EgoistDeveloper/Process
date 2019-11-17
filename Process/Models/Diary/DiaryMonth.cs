using Process.Models.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace Process.Models.Diary
{
    public class DiaryMonth : Month, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<DiaryDay> DiaryDays { get; set; }
    }
}
