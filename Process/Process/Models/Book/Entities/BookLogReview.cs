using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Book
{
    public class BookLogReview : INotifyPropertyChanged
    {
        public BookLogReview()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public string Review { get; set; }
        [Required]
        public long BookLogBookId { get; set; }
    }
}
