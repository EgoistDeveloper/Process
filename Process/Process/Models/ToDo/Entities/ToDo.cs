using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.ToDo
{
    public class ToDo : INotifyPropertyChanged
    {
        public ToDo()
        {
            AddedDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public long ToDoListId { get; set; }
        [Required]
        public string ToDoContent { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
        [Required]
        public bool IsDone { get; set; }
        [Required]
        public bool IsImportant { get; set; }
    }
}