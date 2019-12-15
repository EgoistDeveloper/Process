using System.ComponentModel;
using System.Windows.Input;

namespace Process.Models.ToDo
{
    public class ToDoListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ToDoDoneCount { get; set; }
        public ToDoList ToDoList { get; set; } = new ToDoList();
        public ICommand UpdateToDoListCommand { get; set; }
    }
}
