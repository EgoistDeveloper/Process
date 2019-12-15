using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Models.Book
{
    public class BookLogTopListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BookLogBook BookLogBook { get; set; } = new BookLogBook();
        public BookLogAuthor BookLogAuthor { get; set; }
        public double ReviewRate { get; set; }
    }
}
