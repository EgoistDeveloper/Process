using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diary
{
    public class DiaryBlackWord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string Word { get; set; }
    }
}
