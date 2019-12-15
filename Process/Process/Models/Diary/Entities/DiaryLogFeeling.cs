using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diary
{
    public class DiaryLogFeeling : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long DiaryLogRateId { get; set; }
        [Required]
        public string Feeling { get; set; }
        public int FeelingRate { get; set; }
    }
}
