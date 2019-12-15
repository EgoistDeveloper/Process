using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Pocket
{
    public class PocketBillCategory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
