using Process.Data;
using Process.Models.Book;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Process.Helpers;

namespace Process.ViewModel.Book
{
    public class BookLogAuthorViewModel : WindowViewModel
    {
        public BookLogAuthorViewModel(Window window, BookLogAuthor bookLogAuthor = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 400;
            WindowMinimumWidth = 800;

            Title = bookLogAuthor != null ? $"Update: {bookLogAuthor.FullName}" : "Add New Author";
            BookLogAuthor = bookLogAuthor != null ? bookLogAuthor : new BookLogAuthor();

            CloseCommand = new RelayCommand(p =>
            {
                AddOrUpdate();

                mWindow.Close();
            });

            AddImageCommand = new RelayCommand(p => AddImage());

        }

        #region Commands

        public ICommand AddImageCommand { get; set; }

        #endregion

        #region Properties

        public BookLogAuthor BookLogAuthor { get; set; }

        #endregion

        #region Methods

        public void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(BookLogAuthor.FullName))
            {
                using var db = new AppDbContext();

                _ = BookLogAuthor.Id > 0 ? db.BookLogAuthors.Update(BookLogAuthor) : db.BookLogAuthors.Add(BookLogAuthor);
                db.SaveChanges();
            }

        }

        #endregion

        #region Command Methods

        public void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select Book Image",
                Filter = Settings.ImageFilter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BookLogAuthor.Image = openFileDialog.FileName.PathToBitmapImage();
            }
        }

        #endregion
    }
}
