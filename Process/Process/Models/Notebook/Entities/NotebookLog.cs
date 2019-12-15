using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Notebook
{
    public class NotebookLog : INotifyPropertyChanged
    {
        public NotebookLog()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long NotebookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}