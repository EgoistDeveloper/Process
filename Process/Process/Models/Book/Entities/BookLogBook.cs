using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace Process.Models.Book
{
    public class BookLogBook : INotifyPropertyChanged
    {
        public BookLogBook()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long BookLogAuthorId { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string OriginalTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Pages { get; set; }
        public BitmapImage Image { get; set; }
    }
}