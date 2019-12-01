using Process.Data;
using Process.Helpers;
using Process.Models.Book;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Process.ViewModel.Book
{
    public class BookLogGenreViewModel : WindowViewModel
    {
        public BookLogGenreViewModel(Window window) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 300;
            WindowMinimumWidth = 400;

            AddGenreCommand = new RelayCommand(p => AddGenre());

            LoadBookLogGenres();
        }

        #region Commands

        public ICommand AddGenreCommand { get; set; }

        #endregion

        #region Properties

        public BookLogGenre BookLogGenre { get; set; } = new BookLogGenre();
        public BookLogGenre SelectedBookLogGenre { get; set; }
        public ObservableCollection<BookLogGenre> BookLogGenres { get; set; } = 
            new ObservableCollection<BookLogGenre>();

        #endregion

        #region Methods

        /// <summary>
        /// Load book genres
        /// </summary>
        public void LoadBookLogGenres()
        {
            using var db = new AppDbContext();
            BookLogGenres = db.BookLogGenres.ToObservableCollection();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Save booklog genre
        /// </summary>
        public void AddGenre()
        {
            if (!string.IsNullOrWhiteSpace(BookLogGenre.Genre))
            {
                using var db = new AppDbContext();

                db.BookLogGenres.Add(BookLogGenre);
                db.SaveChanges();

                // add to list
                BookLogGenres.Add(BookLogGenre);
                // select
                SelectedBookLogGenre = BookLogGenre;

                // reset
                BookLogGenre = new BookLogGenre();
            }
        }

        #endregion
    }
}