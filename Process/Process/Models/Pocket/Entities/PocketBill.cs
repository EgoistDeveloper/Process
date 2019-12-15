using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Process.Models.Pocket
{
    public class PocketBill : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string AddedDate { get; set; }
        [Required]
        public string PaymentDate { get; set; }
        public double PaymentAmount { get; set; }
        public int IsPaid { get; set; }
        [Required]
        public long PocketBillCategoryId { get; set; }
    }
}
