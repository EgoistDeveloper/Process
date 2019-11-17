using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Process.Models.Book
{
    public class BookLogBookItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BookLogBook BookLogBook { get; set; } = new BookLogBook();
        public BookLogAuthor BookLogAuthor { get; set; }
        public ObservableCollection<BookLogReview> BookLogReviews { get; set; } = new ObservableCollection<BookLogReview>();
        public ObservableCollection<BookLogBookGenre> BookLogBookGenres { get; set; } = new ObservableCollection<BookLogBookGenre>();
    }
}
