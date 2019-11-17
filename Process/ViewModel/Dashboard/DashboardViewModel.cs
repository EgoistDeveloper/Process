using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Process.Data;
using Process.Models.Dashboard;
using Process.Models.Pocket;
using Microsoft.EntityFrameworkCore;
using System.Windows.Shapes;
using System.Windows.Media;
using Process.Models.Common;
using System.Windows.Controls;
using static Process.DI.DI;
using System.Windows.Input;

namespace Process.ViewModel.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            using var db = new AppDbContext();

            QuickInfos = new List<QuickInfo>
            {
                // Diary Logs
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.DiaryLog,
                    IconData = (System.Windows.Application.Current.FindResource("FountainPenTip") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Blue500Brush") as SolidColorBrush,
                    InfoText1 = $"{db.DiaryLogs.Count(x => EF.Functions.Like(x.Date.Year.ToString(), DateTime.Now.Year.ToString()))} / 365",
                    InfoText2 = "Diary Logs"
                },
                // Review / Book
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.BookLog,
                    IconData = (System.Windows.Application.Current.FindResource("BookOpenPageVariant") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Amber600Brush") as SolidColorBrush,
                    InfoText1 = $"{db.BookLogReviews.Count()} / {db.BookLogBooks.Count()}",
                    InfoText2 = "Review / Book"
                },
                // Done / ToDo
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.ToDoList,
                    IconData = (System.Windows.Application.Current.FindResource("ArrowDecision") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Lime600Brush") as SolidColorBrush,
                    InfoText1 = $"{db.ToDos.Count(x => x.IsDone)} / {db.ToDos.Count()}",
                    InfoText2 = "Done / ToDo"
                },
                // Done / Plan
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.WorkoutLog,
                    IconData = (System.Windows.Application.Current.FindResource("WeightLifter") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Red300Brush") as SolidColorBrush,
                    InfoText1 = $"{db.WorkoutPlans.Count(x => x.IsCompleted)} / {db.WorkoutPlans.Count()}",
                    InfoText2 = "Done / Plan"
                },
                // Notes
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.Notebook,
                    IconData = (System.Windows.Application.Current.FindResource("Notebook") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Teal300Brush") as SolidColorBrush,
                    InfoText1 = $"{db.NotebookLogs.Count()} / {db.Notebooks.Count()}",
                    InfoText2 = "Notes"
                },
                // In / Out
                new QuickInfo
                {
                    ApplicationPage = ApplicationPage.PocketBank,
                    IconData = (System.Windows.Application.Current.FindResource("CashMultiple") as Path)?.Data,
                    IconBackground = System.Windows.Application.Current.FindResource("Purple300Brush") as SolidColorBrush,
                    InfoText1 = $"{db.NotebookLogs.Count()} / {db.Notebooks.Count()}",
                    InfoText2 = "In / Out"
                }
            };

            GoToCommand = new RelayParameterizedCommand(GoTo);
        }


        public List<QuickInfo> QuickInfos { get; set; }

        public ICommand GoToCommand { get; set; }

        public void GoTo(object sender)
        {
            if (sender == null || !(sender is Button button)) return;

            if (!(button.DataContext is QuickInfo item)) return;

            if (ViewModelApplication.CurrentPage != item.ApplicationPage)
            {
                ViewModelApplication.GoToPage(item.ApplicationPage);
            }
        }

    }
}
