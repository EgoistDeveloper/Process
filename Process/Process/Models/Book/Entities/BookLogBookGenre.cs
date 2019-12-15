using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Book
{
    public class BookLogBookGenre : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long BookLogBookId { get; set; }
        [Required]
        public long BookLogGenreId { get; set; }
    }
}