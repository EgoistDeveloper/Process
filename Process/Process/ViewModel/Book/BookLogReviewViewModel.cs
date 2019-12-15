using Process.Data;
using Process.Dialogs.Book;
using Process.Helpers;
using Process.Models.Book;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Process.ViewModel.Book
{
    public class BookLogReviewViewModel : WindowViewModel
    {
        public BookLogReviewViewModel(Window window, BookLogReviewItem bookLogReviewItem = null, BookLogBookItem bookLogBookItem = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 600;
            WindowMinimumWidth = 800;

            Title = bookLogReviewItem != null ? 
                $"Update Review: {bookLogReviewItem.BookLogBookItem.BookLogBook.Title}" : 
                "Add New Book Review";
            BookLogReviewItem = bookLogReviewItem ?? new BookLogReviewItem();

            using var db = new AppDbContext();
            BookLogBooks = db.BookLogBooks.ToObservableCollection();

            if (bookLogBookItem != null)
            {
                SelectedBookLogBook = BookLogBooks.FirstOrDefault(x => x.Id == bookLogBookItem.BookLogBook.Id);
            }

            if (bookLogReviewItem != null)
            {
                SelectedBookLogBook = BookLogBooks.First(x => x.Id == bookLogReviewItem.BookLogBookItem.BookLogBook.Id);
            }

            CloseCommand = new RelayCommand(p =>
            {
                AddOrUpdate();

                mWindow.Close();
            });

            AddBookLogBookCommand = new RelayCommand(p => AddBookLogBook());
            OpenHyperlinkCommand = new RelayParameterizedCommand(OpenHyperlink);
        }

        #region Commands

        public ICommand AddBookLogBookCommand { get; set; }

        /// <summary>
        /// Open hyper link command
        /// </summary>
        public ICommand OpenHyperlinkCommand { get; set; }

        #endregion

        #region Properties

        public BookLogReviewItem BookLogReviewItem { get; set; }
        public ObservableCollection<BookLogBook> BookLogBooks { get; set; } = new ObservableCollection<BookLogBook>();
        public BookLogBook SelectedBookLogBook { get; set; }

        #endregion

        #region Methods

        public void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(BookLogReviewItem.BookLogReview.Review) &&
                BookLogReviewItem.BookLogReview.Rate > 0 && SelectedBookLogBook != null)
            {
                BookLogReviewItem.BookLogBookItem.BookLogBook = SelectedBookLogBook;

                BookLogReviewItem.BookLogReview.BookLogBookId = SelectedBookLogBook.Id;

                using var db = new AppDbContext();

                _ = BookLogReviewItem.BookLogReview.Id > 0 ?
                    db.BookLogReviews.Update(BookLogReviewItem.BookLogReview) :
                    db.BookLogReviews.Add(BookLogReviewItem.BookLogReview);
                db.SaveChanges();
            }
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
                    if (vm.BookLogBookItem.BookLogBook != null && !BookLogBooks.Any(x => x == vm.BookLogBookItem.BookLogBook))
                    {
                        BookLogBooks.Insert(0, vm.BookLogBookItem.BookLogBook);
                    }
                    else
                    {
                        for (int i = 0; i < BookLogBooks.Count; i++)
                        {
                            if (BookLogBooks[i].Id == vm.BookLogBookItem.BookLogBook.Id)
                            {
                                BookLogBooks[i] = vm.BookLogBookItem.BookLogBook;
                                break;
                            }
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new BookLogBookViewModel(dialog));
        }

        /// <summary>
        /// Open hyper link
        /// </summary>
        /// <param name="link"></param>
        public void OpenHyperlink(object link)
        {
            if (!(link is string input)) return;
            System.Diagnostics.Process.Start(input);
        }

        #endregion
    }
}
