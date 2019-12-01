using Process.Data;
using Process.Dialogs.Book;
using Process.Helpers;
using Process.Models.Book;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Process.ViewModel.Book
{
    public class BookLogBookViewModel : WindowViewModel
    {
        public BookLogBookViewModel(Window window, BookLogBookItem bookLogBookItem = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 600;
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
            AddGenreCommand = new RelayCommand(p => AddGenre());

            RemoveBookLogBookGenreCommand = new RelayParameterizedCommand(RemoveBookLogBookGenre);
            LoadAuthors();
        }

        #region Commands

        public ICommand AddImageCommand { get; set; }
        public ICommand AddAuthorCommand { get; set; }
        public ICommand AddGenreCommand { get; set; }

        public ICommand RemoveBookLogBookGenreCommand { get; set; }

        #endregion

        #region Properties

        public BookLogBookItem BookLogBookItem { get; set; }
        public ObservableCollection<BookLogAuthor> BookLogAuthors { get; set; } = new ObservableCollection<BookLogAuthor>();

        #endregion

        #region Methods

        /// <summary>
        /// todo: remove or improve
        /// </summary>
        public void LoadAuthors()
        {
            using var db = new AppDbContext();

            BookLogAuthors = db.BookLogAuthors.ToObservableCollection();
        }

        /// <summary>
        /// Add or update
        /// </summary>
        public void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(BookLogBookItem.BookLogBook.Title) && 
                BookLogBookItem.BookLogBook.Pages > 0 && BookLogBookItem.BookLogAuthor != null)
            {
                BookLogBookItem.BookLogBook.BookLogAuthorId = BookLogBookItem.BookLogAuthor.Id;

                using var db = new AppDbContext();

                _ = BookLogBookItem.BookLogBook.Id > 0 ? 
                    db.BookLogBooks.Update(BookLogBookItem.BookLogBook) : 
                    db.BookLogBooks.Add(BookLogBookItem.BookLogBook);

                db.SaveChanges();

                SaveBookGenres();
            }
        }

        /// <summary>
        /// Save book genres
        /// </summary>
        private void SaveBookGenres()
        {
            if(BookLogBookItem.BookLogBookGenreItems.Count > 0)
            {
                using var db = new AppDbContext();

                foreach (var bookLogBookGenreItem in BookLogBookItem.BookLogBookGenreItems)
                {
                    if (!db.BookLogBookGenres.Any(x => x == bookLogBookGenreItem.BookLogBookGenre))
                    {
                        db.BookLogBookGenres.Add(bookLogBookGenreItem.BookLogBookGenre);
                    }
                }

                db.SaveChanges();
            }
        }
        #endregion

        #region Command Methods

        /// <summary>
        /// Add book image
        /// </summary>
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

        /// <summary>
        /// Add author
        /// </summary>
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

        /// <summary>
        /// Add book genre
        /// </summary>
        public void AddGenre()
        {
            var dialog = new BookLogGenreDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is BookLogGenreViewModel vm)
                {
                    if (vm.SelectedBookLogGenre != null && !BookLogBookItem.BookLogBookGenreItems.Any(x => x.BookLogGenre == vm.SelectedBookLogGenre))
                    {
                        BookLogBookItem.BookLogBookGenreItems.Insert(0, new BookLogBookGenreItem 
                        { 
                            BookLogBookGenre = new BookLogBookGenre 
                            { 
                                BookLogGenreId = vm.SelectedBookLogGenre.Id
                            },
                            BookLogGenre = vm.SelectedBookLogGenre
                        });
                    }
                }
            };

            dialog.ShowDialogWindow(new BookLogGenreViewModel(dialog), mWindow);
        }

        /// <summary>
        /// Remove book genre
        /// </summary>
        /// <param name="sender"></param>
        public void RemoveBookLogBookGenre(object sender)
        {
            var bookLogBookGenreItem = (sender as Button).DataContext as BookLogBookGenreItem;

            using var db = new AppDbContext();

            db.BookLogBookGenres.Remove(bookLogBookGenreItem.BookLogBookGenre);
            db.SaveChanges();

            BookLogBookItem.BookLogBookGenreItems.Remove(bookLogBookGenreItem);
        }
        #endregion
    }
}