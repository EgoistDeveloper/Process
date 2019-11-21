using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs.Book;
using Process.Helpers;
using Process.Models.Book;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Process.ViewModel.Book
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            AddBookLogBookCommand = new RelayCommand(p => AddBookLogBook());
            AddBookLogReviewCommand = new RelayCommand(p => AddBookLogReview());
            EditBookLogReviewCommand = new RelayParameterizedCommand(EditBookLogReview);
            LoadBooks();
        }

        #region Commands

        public ICommand AddBookLogBookCommand { get; set; }
        public ICommand AddBookLogReviewCommand { get; set; }
        public ICommand EditBookLogReviewCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<BookLogBookItem> BookLogBookItems { get; set; } = new ObservableCollection<BookLogBookItem>();
        public ObservableCollection<BookLogReviewItem> BookLogReviewItems { get; set; } = new ObservableCollection<BookLogReviewItem>();

        #endregion

        #region Methods

        public void LoadBookReviews()
        {
            using var db = new AppDbContext();
            BookLogReviewItems = db.BookLogReviews.Select(bookLogReview => new BookLogReviewItem
            {
                BookLogReview = bookLogReview,
                BookLogBookItem = db.BookLogBooks.Select(bookLogBook => new BookLogBookItem
                {
                    BookLogBook = bookLogBook,
                    BookLogAuthor = db.BookLogAuthors.SingleOrDefault(x => x.Id == bookLogBook.BookLogAuthorId)
                }).First()
            }).OrderByDescending(x => x.BookLogReview.Id).ToObservableCollection();
        }

        public void LoadBooks()
        {
            using var db = new AppDbContext();
            BookLogBookItems = db.BookLogBooks.Select(bookLogBook => new BookLogBookItem
            {
                BookLogBook = bookLogBook,
                BookLogAuthor = db.BookLogAuthors.SingleOrDefault(x => x.Id == bookLogBook.BookLogAuthorId),
                BookLogReviews = db.BookLogReviews.Where(x => x.BookLogBookId == bookLogBook.Id)
                .ToObservableCollection(),
                BookLogBookGenres = db.BookLogBookGenres.Where(x => x.BookLogBookId == bookLogBook.Id)
                .Select(x => new BookLogBookGenre
                {
                    Id = x.Id,
                    BookLogBookId = x.BookLogBookId,
                    BookLogGenreId = x.BookLogGenreId,
                    BookLogGenre = db.BookLogGenres.FirstOrDefault(v => v.Id == x.BookLogGenreId)
                }).ToObservableCollection()
            })
            .OrderByDescending(x => x.BookLogBook.Id)
            .ToObservableCollection();

            LoadBookReviews();
        }

        #endregion

        #region Command Methods

        public void AddBookLogBook()
        {
            var dialog = new BookLogBookDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is BookLogBookViewModel vm)
                {
                    if (vm.BookLogBookItem.BookLogBook != null && !BookLogBookItems.Any(x => x == vm.BookLogBookItem))
                    {
                        BookLogBookItems.Insert(0, vm.BookLogBookItem);
                    }
                    else
                    {
                        for (int i = 0; i < BookLogBookItems.Count; i++)
                        {
                            if (BookLogBookItems[i].BookLogBook.Id == vm.BookLogBookItem.BookLogBook.Id)
                            {
                                BookLogBookItems[i] = vm.BookLogBookItem;
                                break;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new BookLogBookViewModel(dialog));
        }

        public void AddBookLogReview()
        {
            var dialog = new BookLogReviewDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is BookLogReviewViewModel vm)
                {
                    if (vm.BookLogReviewItem != null && !BookLogReviewItems.Any(x => x == vm.BookLogReviewItem))
                    {
                        BookLogReviewItems.Insert(0, vm.BookLogReviewItem);
                    }
                    else
                    {
                        for (int i = 0; i < BookLogReviewItems.Count; i++)
                        {
                            if (BookLogReviewItems[i].BookLogReview.Id == vm.BookLogReviewItem.BookLogReview.Id)
                            {
                                BookLogReviewItems[i] = vm.BookLogReviewItem;
                                break;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new BookLogReviewViewModel(dialog));
        }

        public void EditBookLogReview(object sender)
        {
            var bookLogReviewItem = (sender as Button).DataContext as BookLogReviewItem;

            var dialog = new BookLogReviewDialog();

            dialog.Closing += (sender, args) =>
            {
                if (dialog.DataContext is BookLogReviewViewModel vm)
                {
                    bookLogReviewItem = vm.BookLogReviewItem;                }
            };

            dialog.ShowDialogWindow(new BookLogReviewViewModel(dialog, bookLogReviewItem));
        }

        #endregion
    }
}