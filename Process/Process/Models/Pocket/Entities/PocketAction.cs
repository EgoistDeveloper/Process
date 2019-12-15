using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Pocket
{
    public class PocketAction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long PocketCategoryId { get; set; }
    }
}
