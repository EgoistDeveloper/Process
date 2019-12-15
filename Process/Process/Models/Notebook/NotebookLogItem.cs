using System.ComponentModel;
using System.Windows.Input;

namespace Process.Models.Notebook
{
    public class NotebookLogItem : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand EditNotebookLogCommand { get; set; }
        public ICommand DeleteNotebookLogCommand { get; set; }

        public NotebookLog NotebookLog { get; set; }
    }
}