using System.Collections.ObjectModel;

namespace Process.Models.Notebook
{
    public class NotebookItem
    {
        public Notebook Notebook { get; set; }
        public ObservableCollection<NotebookLog> NotebookLogs { get; set; } = new ObservableCollection<NotebookLog>();
        public NotebookLog SelectedNotebookLog { get; set; }
    }
}
