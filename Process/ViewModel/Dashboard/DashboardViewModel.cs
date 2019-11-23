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
using System.Collections.ObjectModel;
using Process.Models.Book;
using Process.Models.ToDo;
using Process.Helpers;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using LiveCharts;
using LiveCharts.Wpf;

namespace Process.ViewModel.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            Init();

            GoToCommand = new RelayParameterizedCommand(GoTo);
            LoadedCommand = new RelayCommand(p => Init());
            SetIsDoneCommand = new RelayParameterizedCommand(SetIsDone);
            SetIsImportantCommand = new RelayParameterizedCommand(SetIsImportant);
        }

        public ICommand LoadedCommand { get; set; }

        public void Init()
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

            LoadBookList();
            LoadLastToDos();
            LoadWeather();
        }

        public List<QuickInfo> QuickInfos { get; set; }
        public ObservableCollection<BookLogTopListItem> BookLogTopListItems { get; set; }
        public ObservableCollection<ToDoLastItem> ToDoLastItems { get; set; }
        public ObservableCollection<WeatherInfo> WeatherInfos { get; set; } = new ObservableCollection<WeatherInfo>();
        public WeatherInfo TodayWeatherInfo { get; set; }

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public List<string> Labels { get; set; } = new List<string>();
        public Func<double, string> YFormatter { get; set; }

        public ICommand GoToCommand { get; set; }
        public ICommand SetIsDoneCommand { get; set; }
        public ICommand SetIsImportantCommand { get; set; }

        public void GoTo(object sender)
        {
            if (sender == null || !(sender is Button button)) return;

            if (!(button.DataContext is QuickInfo item)) return;

            if (ViewModelApplication.CurrentPage != item.ApplicationPage)
            {
                ViewModelApplication.GoToPage(item.ApplicationPage);
            }
        }

        public void LoadBookList()
        {
            using var db = new AppDbContext();

            BookLogTopListItems = db.BookLogBooks.Select(bookLogBook => new BookLogTopListItem
            {
                BookLogBook = bookLogBook,
                BookLogAuthor = db.BookLogAuthors.SingleOrDefault(x => x.Id == bookLogBook.BookLogAuthorId),
                ReviewRate = db.BookLogReviews.Where(x => x.BookLogBookId == bookLogBook.Id)
                .Sum(x => x.Rate) < 1 ? 1 : db.BookLogReviews.Where(x => x.BookLogBookId == bookLogBook.Id)
                .Sum(x => x.Rate) / db.BookLogReviews
                .Where(x => x.BookLogBookId == bookLogBook.Id).Count() < 1 ? 1 : db.BookLogReviews
                .Where(x => x.BookLogBookId == bookLogBook.Id).Count(),
            })
            .OrderByDescending(x => x.ReviewRate)
            .Take(10)
            .ToObservableCollection();
        }

        public void LoadLastToDos()
        {
            using var db = new AppDbContext();

            ToDoLastItems = db.ToDos.Select(toDo => new ToDoLastItem 
            { 
                ToDo= toDo,
                ToDoList = db.ToDoLists.First(x => x.Id == toDo.ToDoListId)
            })
            .Take(20)
            .ToObservableCollection();
        }

        /// <summary>
        /// Set Is Done ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void SetIsDone(object sender)
        {
            var toDo = ((sender as CheckBox).DataContext as Models.ToDo.ToDoLastItem).ToDo;

            using var db = new AppDbContext();

            db.ToDos.Update(toDo);
            db.SaveChanges();
        }

        /// <summary>
        /// Set Is Important ToDo
        /// </summary>
        /// <param name="sender"></param>
        public void SetIsImportant(object sender)
        {
            var toDo = ((sender as CheckBox).DataContext as Models.ToDo.ToDoLastItem).ToDo;

            using var db = new AppDbContext();
            db.ToDos.Update(toDo);
            db.SaveChanges();
        }

        public void LoadWeather()
        {
            var apiKey = ViewModelApplication.AppSettings.OpenWeatherApiKey;
            var countryCode = ViewModelApplication.AppSettings.OpenWeatherCountry;
            var city = ViewModelApplication.AppSettings.OpenWeatherCity;
            var respose = Helpers.HttpHelpers.Get($"https://api.openweathermap.org/data/2.5/forecast?q={city},{countryCode}&units=metric&appid={apiKey}");

            if (!string.IsNullOrEmpty(respose))
            {
                var Temperatures = JsonConvert.DeserializeObject<Temperatures>(respose);

                if (Temperatures.List.Length > 0)
                {
                    SeriesCollection = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = "Temperature",
                            Values = new ChartValues<double> { }
                        }
                    };

                    var weatherInfos = new ObservableCollection<WeatherInfo>();

                    var days = Temperatures.List.GroupBy(x => x.DtTxt.Date);

                    foreach (var day in days)
                    {
                        var firstDayItem = day.First();
                        weatherInfos.Add(new WeatherInfo
                        {
                            Date = firstDayItem.DtTxt.Date,
                            Temperature = firstDayItem.Main.Temp,
                            Description = firstDayItem.Weather.FirstOrDefault().Description,
                            Weather = Settings.CultureInfo.TextInfo.ToTitleCase(GetEnumMemberAttrValue(day.First().Weather[0].Description)),
                            Temperatures = days.SelectMany(group => group).ToList()
                        });

                        SeriesCollection[0].Values.Add(firstDayItem.Main.Temp);
                        Labels.Add(firstDayItem.DtTxt.Date.ToShortDateString());
                    }

                    WeatherInfos = weatherInfos;
                    TodayWeatherInfo = weatherInfos.FirstOrDefault(x => x.Date == DateTime.Now.Date);
                }
            }
        }

        public string GetEnumMemberAttrValue<T>(T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if (attr != null)
            {
                return attr.Value;
            }

            return null;
        }
    }
}
