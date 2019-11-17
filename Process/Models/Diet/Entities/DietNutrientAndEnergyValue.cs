using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Diet
{
    public class DietNutrientAndEnergyValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; } = DateTime.Now;
        [Required]
        public long DietFoodId { get; set; }
        [Required]
        public double Calorie { get; set; }
        [Required]
        public double CalorieDailyVal { get; set; }

        public virtual ObservableCollection<DietMineralValue> DietMineralValues { get; set; } = new ObservableCollection<DietMineralValue>();
        public virtual ObservableCollection<DietVitaminValue> DietVitaminValues { get; set; } = new ObservableCollection<DietVitaminValue>();
        public virtual ObservableCollection<DietNutrientTypeValue> DietNutrientTypeValues { get; set; } = new ObservableCollection<DietNutrientTypeValue>();
    }
}
