using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diet
{
    public class DietMineralValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; } = DateTime.Now;
        [Required]
        public long DietNutrientAndEnergyValueId { get; set; }
        [Required]
        public Mineral Mineral { get; set; }
        [Required]
        public Unit Unit { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double DailyValue { get; set; }
    }
}
