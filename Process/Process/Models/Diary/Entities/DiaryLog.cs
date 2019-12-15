using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diary
{
    public class DiaryLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
        [Required]
        public string LogContent { get; set; }
        [Required]
        public long LogContentLength { get; set; }
        public DiaryLogRate DiaryLogRate { get; set; }

        public virtual List<DiaryHistoryLog> DiaryHistoryLogs { get; set; } = new List<DiaryHistoryLog>();
    }
}
