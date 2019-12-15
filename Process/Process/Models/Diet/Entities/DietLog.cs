using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diet
{
    public class DietLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long DietFoodId { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Note { get; set; }
    }
}
