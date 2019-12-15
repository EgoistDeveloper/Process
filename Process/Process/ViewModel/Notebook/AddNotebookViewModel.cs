using Process.Data;
using System.Windows;

namespace Process.ViewModel.Notebook
{
    public class AddNotebookViewModel : WindowViewModel
    {
        public AddNotebookViewModel(Window window, Models.Notebook.Notebook notebook = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 250;
            WindowMinimumWidth = 400;
            Title = notebook != null ? $"Edit: {notebook.Title}" : "Add New Notebook";

            Notebook = notebook ?? new Models.Notebook.Notebook();

            CloseCommand = new RelayCommand(p => CloseWindow());
        }

        #region Properties

        public Models.Notebook.Notebook Notebook { get; set; }

        #endregion

        #region Command Methods

        public void CloseWindow()
        {
            if (!string.IsNullOrWhiteSpace(Notebook.Title))
            {
                using var db = new AppDbContext();

                _ = Notebook.Id > 0 ? db.Notebooks.Update(Notebook) : db.Notebooks.Add(Notebook);
                db.SaveChanges();
            }

            mWindow.Close();
        }

        #endregion
    }
}