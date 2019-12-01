using System.ComponentModel;

namespace Process.Models.Book
{
    public class BookLogReviewItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BookLogReview BookLogReview { get; set; } = new BookLogReview();
        public BookLogBookItem BookLogBookItem { get; set; } = new BookLogBookItem();
    }
}
