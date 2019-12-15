using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace Process.Models.Book
{
    public class BookLogAuthor : INotifyPropertyChanged
    {
        public BookLogAuthor()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string FullName { get; set; }
        public BitmapImage Image { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string GoodReads { get; set; }

        //public ObservableCollection<BookLogBook> BookLogBooks { get; set; } = new ObservableCollection<BookLogBook>();
    }
}
