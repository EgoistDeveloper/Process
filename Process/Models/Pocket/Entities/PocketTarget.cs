using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Process.Models.Pocket
{
    public class PocketTarget : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
        [Required]
        public string Target { get; set; }
        [Required]
        public int BudgetPercent { get; set; }
    }
}
