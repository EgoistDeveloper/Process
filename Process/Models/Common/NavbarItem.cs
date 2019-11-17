using System.Windows.Media;

namespace Process.Models.Common
{
    public class NavbarItem
    {
        public ApplicationPage ApplicationPage { get; set; }
        public Geometry IconData { get; set; }
        public bool IsChecked { get; set; }
    }
}
