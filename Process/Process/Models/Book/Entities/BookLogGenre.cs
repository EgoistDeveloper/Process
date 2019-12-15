using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Book
{
    public class BookLogGenre : INotifyPropertyChanged
    {
        public BookLogGenre()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string Genre { get; set; }
    }
}
