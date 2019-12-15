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
            LoadNotebookItems();
        }

        #region Commands

        public ICommand AddNotebookCommand { get; set; }

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
                NotebookLogItems = db.NotebookLogs.Where(nLog => nLog.NotebookId == notebook.Id)
                .Select(nLog => new NotebookLogItem 
                {
                    NotebookLog = nLog,

                    EditNotebookLogCommand = new RelayParameterizedCommand(EditNotebookLog),
                    DeleteNotebookLogCommand = new RelayParameterizedCommand(DeleteNotebookLog)
                })
                .OrderByDescending(x => x.NotebookLog.Id).ToObservableCollection(),

                EditNotebookCommand = new RelayParameterizedCommand(EditNotebook),
                DeleteNotebookCommand = new RelayParameterizedCommand(DeleteNotebook),
                AddNotebookLogCommand = new RelayParameterizedCommand(AddNotebookLog)
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
                            Notebook = vm.Notebook,

                            EditNotebookCommand = new RelayParameterizedCommand(EditNotebook),
                            DeleteNotebookCommand = new RelayParameterizedCommand(DeleteNotebook)
                        });
                    }
                }
            };

            dialog.ShowDialogWindow(new AddNotebookViewModel(dialog));
        }

        /// <summary>
        /// Show edit notebook dialog
        /// </summary>
        public void EditNotebook(object sender)
        {
            var notebook = (Models.Notebook.Notebook)sender;

            var dialog = new NotebookDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is AddNotebookViewModel vm && vm.Notebook != null)
                {
                    SelectedNotebookItem.Notebook = vm.Notebook;
                }
            };

            dialog.ShowDialogWindow(new AddNotebookViewModel(dialog, notebook));
        }

        /// <summary>
        /// Delete notebook dialog
        /// </summary>
        public void DeleteNotebook(object sender)
        {
            var notebook = (Models.Notebook.Notebook)sender;

            var dialog = new DeleteDialog();

            dialog.Closing += (send, args) =>
            {
                if (dialog.DataContext is DeleteDialogViewModel vm && vm.Result)
                {
                    using var db = new AppDbContext();

                    db.Notebooks.Remove(notebook);
                    db.SaveChanges();

                    if (NotebookItems.Any(x => x.Notebook == notebook))
                    {
                        NotebookItems.Remove(NotebookItems.First(x => x.Notebook == notebook));
                        return;
                    }
                }
            };

            dialog.ShowDialogWindow(new DeleteDialogViewModel(dialog, "Delete Notebook", notebook.Title));
        }

        /// <summary>
        /// Add new notebook log
        /// </summary>
        public void AddNotebookLog(object sender)
        {
            var selecteNotebookLogItem = (NotebookLogItem)sender;

            var dialog = new NotebookLogDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is NotebookLogViewModel vm && vm.NotebookLog != null)
                {
                    var notebookLogItem = new NotebookLogItem
                    {
                        NotebookLog = vm.NotebookLog,

                        EditNotebookLogCommand = new RelayParameterizedCommand(EditNotebookLog),
                        DeleteNotebookLogCommand = new RelayParameterizedCommand(DeleteNotebookLog)
                    };

                    SelectedNotebookItem.NotebookLogItems.Insert(0, notebookLogItem);
                }
            };

            dialog.ShowDialogWindow(new NotebookLogViewModel(dialog, new NotebookLog 
            {
                NotebookId = SelectedNotebookItem.Notebook.Id
            }));
        }

        /// <summary>
        /// Edit notebook log
        /// </summary>
        /// <param name="sender">Button</param>
        public void EditNotebookLog(object sender)
        {
            var notebookLog = (NotebookLog)sender;

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
            var notebookLog = (NotebookLog)sender;

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
                        if (NotebookItems[i].NotebookLogItems.Any(x => x.NotebookLog == notebookLog))
                        {
                            NotebookItems[i].NotebookLogItems.Remove(NotebookItems[i].NotebookLogItems.First(x => x.NotebookLog == notebookLog));
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