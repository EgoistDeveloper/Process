using Process.Data;
using Process.Models.Book;
using System.Windows;

namespace Process.ViewModel.Book
{
    public class BookLogGenreViewModel : WindowViewModel
    {
        public BookLogGenreViewModel(Window window, BookLogGenre bookLogGenre = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 200;
            WindowMinimumWidth = 400;

            Title = bookLogGenre != null ? $"Update: {bookLogGenre.Genre}" : "Add New Genre";
            BookLogGenre = bookLogGenre ?? new BookLogGenre();

            CloseCommand = new RelayCommand(p =>
            {
                AddOrUpdate();

                mWindow.Close();
            });
        }

        #region Properties

        public BookLogGenre BookLogGenre { get; set; }

        #endregion

        #region Methods

        public void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(BookLogGenre.Genre))
            {
                using var db = new AppDbContext();

                _ = BookLogGenre.Id > 0 ? db.BookLogGenres.Update(BookLogGenre) : db.BookLogGenres.Add(BookLogGenre);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
