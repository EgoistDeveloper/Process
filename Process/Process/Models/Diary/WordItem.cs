using System.ComponentModel;
using System.Windows.Input;

namespace Process.Models.Diary
{
    public class WordItem : DiaryBlackWord, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Count { get; set; }

        public ICommand Command { get; set; }
    }
}
