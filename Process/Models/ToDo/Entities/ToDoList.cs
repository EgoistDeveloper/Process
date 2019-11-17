using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.ToDo
{
    public class ToDoList : INotifyPropertyChanged
    {
        public ToDoList()
        {
            LastUpdateDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }

        public virtual ObservableCollection<ToDo> ToDos { get; set; } = new ObservableCollection<ToDo>();
    }
}
