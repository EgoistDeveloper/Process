/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:KOR.SysInfo.Core"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Process.ViewModel.App;
using Process.ViewModel.Book;
using Process.ViewModel.Dashboard;
using Process.ViewModel.Diary;
using Process.ViewModel.Diet;
using Process.ViewModel.Notebook;
using Process.ViewModel.PocketBank;
using Process.ViewModel.ToDo;
using Process.ViewModel.Workout;
using static Process.DI.DI;
//using Microsoft.Practices.ServiceLocation;

namespace Process.ViewModel.Base
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
            }
            else
            {
                SimpleIoc.Default.Register<DashboardViewModel>();
                SimpleIoc.Default.Register<DiaryLogViewModel>();
                SimpleIoc.Default.Register<NavbarViewModel>();
                SimpleIoc.Default.Register<ToDoLogViewModel>();
                SimpleIoc.Default.Register<WorkoutLogViewModel>();
                SimpleIoc.Default.Register<PocketBankViewModel>();
                SimpleIoc.Default.Register<DietLogViewModel>();
                SimpleIoc.Default.Register<BookLogViewModel>();
                SimpleIoc.Default.Register<NotebookViewModel>();
                SimpleIoc.Default.Register<DiaryLogOfDayViewModel>();
            }
        }

        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public ApplicationViewModel ApplicationViewModel => ViewModelApplication;

        #endregion

        public DashboardViewModel DashboardVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DashboardViewModel>();
            }
        }

        public DiaryLogViewModel DiaryLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DiaryLogViewModel>();
            }
        }

        public ToDoLogViewModel ToDoLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ToDoLogViewModel>();
            }
        }

        public WorkoutLogViewModel WorkoutLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WorkoutLogViewModel>();
            }
        }

        public NavbarViewModel NavbarVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NavbarViewModel>();
            }
        }


        public PocketBankViewModel PocketBankVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PocketBankViewModel>();
            }
        }

        public DietLogViewModel DietLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DietLogViewModel>();
            }
        }

        public BookLogViewModel BookLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookLogViewModel>();
            }
        }

        public NotebookViewModel NotebookVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NotebookViewModel>();
            }
        }

        public DiaryLogOfDayViewModel DiaryLogOfDayVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DiaryLogOfDayViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}