using Process.Models.Common;
using System.ComponentModel;
using System.Windows.Media;

namespace Process.Models.Dashboard
{
    public class QuickInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ApplicationPage ApplicationPage { get; set; }
        public Geometry IconData { get; set; }
        public SolidColorBrush IconBackground { get; set; }
        public string InfoText1 { get; set; }
        public string InfoText2 { get; set; }
    }
}
