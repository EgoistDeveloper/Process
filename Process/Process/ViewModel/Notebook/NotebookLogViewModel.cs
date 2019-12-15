using Process.Data;
using Process.Models.Notebook;
using System.Windows;

namespace Process.ViewModel.Notebook
{
    public class NotebookLogViewModel : WindowViewModel
    {
        public NotebookLogViewModel(Window window, NotebookLog notebookLog) : base(window)
        {
            mWindow = window;
            Title = notebookLog != null && !(string.IsNullOrWhiteSpace(notebookLog.Title)) ? $"Edit: {notebookLog.Title}" : "Add Notebook Log";

            NotebookLog = notebookLog;

            CloseCommand = new RelayCommand(p => AddOrUpdate());
        }

        #region Properties

        public NotebookLog NotebookLog { get; set; }

        #endregion

        #region Command Methods

        private void AddOrUpdate()
        {
            if (!string.IsNullOrWhiteSpace(NotebookLog.Title) && !string.IsNullOrWhiteSpace(NotebookLog.Content))
            {
                using var db = new AppDbContext();

                _ = NotebookLog.Id > 0 ? db.NotebookLogs.Update(NotebookLog) : db.NotebookLogs.Add(NotebookLog);
                db.SaveChanges();
            }

            mWindow.Close();
        }

        #endregion
    }
}