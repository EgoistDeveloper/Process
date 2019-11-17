using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diary
{
    public class DiaryHistoryLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        public long DiaryLogId { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string LogContent { get; set; }
    }
}
