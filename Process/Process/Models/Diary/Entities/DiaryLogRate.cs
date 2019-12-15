using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diary
{
    public class DiaryLogRate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long DiaryLogId { get; set; }
        public int DayRate { get; set; }
        public string DayKeywords { get; set; }

        public virtual List<DiaryLogFeeling> DiaryLogFeelings { get; set; } = new List<DiaryLogFeeling>();
    }
}
