using Process.Data;
using Process.Dialogs.Book;
using Process.Helpers;
using Process.Models.Book;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Process.ViewModel.Book
{
    public class BookLogBookViewModel : WindowViewModel
    {
        public BookLogBookViewModel(Window window, BookLogBookItem bookLogBookItem = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 400;
            WindowMinimumWidth = 800;

            Title = bookLogBookItem != null ? $"Update: {bookLogBookItem.BookLogBook.Title}" : "Add New Book";
            BookLogBookItem = bookLogBookItem != null ? bookLogBookItem : new BookLogBookItem();

            CloseCommand = new RelayCommand(p =>
            {
                AddOrUpdate();

                mWindow.Close();
            });

            AddImageCommand = new RelayCommand(p => AddImage());
            AddAuthorCommand = new RelayCommand(p => AddAuthor());

            LoadAuthors();
        }

        #region Commands

        public ICommand AddImageCommand { get; set; }

        public ICommand AddAuthorCommand { get; set; }

        #endregion

        #region Properties

        public BookLogBookItem BookLogBookItem { get; set; }
        public ObservableCollection<BookLogAuthor> BookLogAuthors { get; set; } = new ObservableCollection<BookLogAuthor>();

        #endregion

        #region Methods

        public void LoadAuthors()
        {
            using var db = new AppDbContext();

            BookLogAuthors = db.BookLogAuthors.ToObservableCollection();
        }

        public void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(BookLogBookItem.BookLogBook.Title) && 
                BookLogBookItem.BookLogBook.Pages > 0 && BookLogBookItem.BookLogAuthor != null)
            {
                BookLogBookItem.BookLogBook.BookLogAuthorId = BookLogBookItem.BookLogAuthor.Id;

                using var db = new AppDbContext();

                _ = BookLogBookItem.BookLogBook.Id > 0 ? db.BookLogBooks.Update(BookLogBookItem.BookLogBook) : db.BookLogBooks.Add(BookLogBookItem.BookLogBook);
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
                BookLogBookItem.BookLogBook.Image = openFileDialog.FileName.PathToBitmapImage();
            }
        }

        public void AddAuthor()
        {
            var dialog = new BookLogAuthorDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is BookLogAuthorViewModel vm)
                {
                    if (vm.BookLogAuthor != null && !BookLogAuthors.Any(x => x == vm.BookLogAuthor))
                    {
                        BookLogAuthors.Insert(0, vm.BookLogAuthor);
                    }
                    else
                    {
                        for (int i = 0; i < BookLogAuthors.Count; i++)
                        {
                            if (BookLogAuthors[i].Id == vm.BookLogAuthor.Id)
                            {
                                BookLogAuthors[i] = vm.BookLogAuthor;
                                break;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new BookLogAuthorViewModel(dialog));
        }

        #endregion
    }
}