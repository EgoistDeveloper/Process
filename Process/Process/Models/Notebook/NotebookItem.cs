using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Process.Models.Notebook
{
    public class NotebookItem : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand EditNotebookCommand { get; set; }
        public ICommand DeleteNotebookCommand { get; set; }
        public ICommand AddNotebookLogCommand { get; set; }

        public Notebook Notebook { get; set; }
        public ObservableCollection<NotebookLogItem> NotebookLogItems { get; set; } = new ObservableCollection<NotebookLogItem>();
        public NotebookLogItem SelectedNotebookLogItem { get; set; }
    }
}