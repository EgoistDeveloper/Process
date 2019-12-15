using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Pocket
{
    public class PocketInOut : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public PocketInOutType Type { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public long PocketActionId { get; set; }
    }
}
