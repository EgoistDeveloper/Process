using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using Process.Models.Common;
using static Process.DI.DI;

namespace Process.ViewModel.App
{
    public class NavbarViewModel : ViewModelBase
    {
        public NavbarViewModel()
        {
            NavbarItems = new List<NavbarItem>()
            {
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Dashboard,
                    IconData = (System.Windows.Application.Current.FindResource("ViewDashboardOutline") as Path)?.Data,
                    IsChecked = true
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.DiaryLog,
                    IconData = (System.Windows.Application.Current.FindResource("FountainPenTip") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.ToDoList,
                    IconData = (System.Windows.Application.Current.FindResource("ArrowDecision") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.DietLog,
                    IconData = (System.Windows.Application.Current.FindResource("FoodForkDrink") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.WorkoutLog,
                    IconData = (System.Windows.Application.Current.FindResource("WeightLifter") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.PocketBank,
                    IconData = (System.Windows.Application.Current.FindResource("CashMultiple") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.BookLog,
                    IconData = (System.Windows.Application.Current.FindResource("BookOpenPageVariant") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Notebook,
                    IconData = (System.Windows.Application.Current.FindResource("Notebook") as Path)?.Data
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Calendar,
                    IconData = (System.Windows.Application.Current.FindResource("CalendarMultipleCheck") as Path)?.Data
                }
            };

            GoToCommand = new RelayParameterizedCommand(GoTo);
        }

        public List<NavbarItem> NavbarItems { get; set; }

        public ICommand GoToCommand { get; set; }

        public void GoTo(object sender)
        {
            if (sender == null || !(sender is ToggleButton toggleButton)) return;

            if (!(toggleButton.DataContext is NavbarItem item)) return;

            if (ViewModelApplication.CurrentPage != item.ApplicationPage)
            {
                ViewModelApplication.GoToPage(item.ApplicationPage);
            }
        }
    }
}