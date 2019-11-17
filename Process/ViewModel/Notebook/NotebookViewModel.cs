using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs;
using Process.Dialogs.Notebook;
using Process.Helpers;
using Process.Models.Notebook;
using Process.ViewModel.App;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Process.ViewModel.Notebook
{
    public class NotebookViewModel : ViewModelBase
    {
        public NotebookViewModel()
        {
            AddNotebookCommand = new RelayCommand(p => AddNotebook());
            AddNotebookLogCommand = new RelayCommand(p => AddNotebookLog());
            EditNotebookCommand = new RelayCommand(p => EditNotebook());
            EditNotebookLogCommand = new RelayParameterizedCommand(EditNotebookLog);
            DeleteNotebookLogCommand = new RelayParameterizedCommand(DeleteNotebookLog);

            LoadNotebookItems();
        }

        #region Commands

        public ICommand AddNotebookCommand { get; set; }
        public ICommand EditNotebookCommand { get; set; }
        public ICommand AddNotebookLogCommand { get; set; }
        public ICommand EditNotebookLogCommand { get; set; }
        public ICommand DeleteNotebookLogCommand { get; set; }
        #endregion

        #region Properties

        public ObservableCollection<NotebookItem> NotebookItems { get; set; } = new ObservableCollection<NotebookItem>();
        public NotebookItem SelectedNotebookItem { get; set; }

        #endregion

        #region Methodds

        /// <summary>
        /// Load notebooks
        /// </summary>
        public void LoadNotebookItems()
        {
            using var db = new AppDbContext();
            NotebookItems = db.Notebooks.Select(notebook => new NotebookItem
            {
                Notebook = notebook,
                NotebookLogs = db.NotebookLogs.Where(nLog => nLog.NotebookId == notebook.Id)
                .OrderByDescending(x => x.Id).ToObservableCollection()
            }).OrderByDescending(x => x.Notebook.Id).ToObservableCollection();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Add new notebook dialog
        /// </summary>
        public void AddNotebook()
        {
            var dialog = new NotebookDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is AddNotebookViewModel vm && vm.Notebook != null)
                {
                    if (!NotebookItems.Any(x => x.Notebook == vm.Notebook))
                    {
                        NotebookItems.Insert(0, new NotebookItem
                        {
                            Notebook = vm.Notebook
                        });
                    }
                }
            };

            dialog.ShowDialogWindow(new AddNotebookViewModel(dialog));
        }

        /// <summary>
        /// Show edit notebook dialog
        /// </summary>
        public void EditNotebook()
        {
            if (SelectedNotebookItem == null) return;

            var dialog = new NotebookDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is AddNotebookViewModel vm && vm.Notebook != null)
                {
                    SelectedNotebookItem.Notebook = vm.Notebook;
                }
            };

            dialog.ShowDialogWindow(new AddNotebookViewModel(dialog, SelectedNotebookItem.Notebook));
        }

        /// <summary>
        /// Add new notebook log
        /// </summary>
        public void AddNotebookLog()
        {
            var dialog = new NotebookLogDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is NotebookLogViewModel vm && vm.NotebookLog != null)
                {
                    SelectedNotebookItem.NotebookLogs.Insert(0, vm.NotebookLog);
                }
            };

            dialog.ShowDialogWindow(new NotebookLogViewModel(dialog, new NotebookLog 
            {
                NotebookId = SelectedNotebookItem.Notebook.Id
            }));
        }

        /// <summary>
        /// Edite notebook log
        /// </summary>
        /// <param name="sender">Button</param>
        public void EditNotebookLog(object sender)
        {
            var notebookLog = (sender as Button).DataContext as NotebookLog;

            var dialog = new NotebookLogDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is NotebookLogViewModel vm && vm.NotebookLog != null)
                {
                    notebookLog = vm.NotebookLog;
                }
            };

            dialog.ShowDialogWindow(new NotebookLogViewModel(dialog, notebookLog));
        }

        /// <summary>
        /// Delete selected notebook log
        /// </summary>
        /// <param name="sender"></param>
        public void DeleteNotebookLog(object sender)
        {
            var notebookLog = (sender as Button).DataContext as NotebookLog;

            var dialog = new DeleteDialog();

            dialog.Closing += (send, args) =>
            {
                if (dialog.DataContext is DeleteDialogViewModel vm && vm.Result)
                {
                    using var db = new AppDbContext();

                    db.NotebookLogs.Remove(notebookLog);
                    db.SaveChanges();

                    for (int i = 0; i < NotebookItems.Count; i++)
                    {
                        if (NotebookItems[i].NotebookLogs.Any(x => x == notebookLog))
                        {
                            NotebookItems[i].NotebookLogs.Remove(notebookLog);
                            return;
                        }
                    }
                }
            };

            dialog.ShowDialogWindow(new DeleteDialogViewModel(dialog, "Delete Notebook Log", notebookLog.Title));
        }

        #endregion
    }
}