using System;
using System.Threading;
using Dna;
using Process.DI;
using Process.Models.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Process.Data;
using Process.Dialogs;
using Process.ViewModel;
using Process.ViewModel.App;
using static Process.DI.DI;
using System.Linq;

namespace Process
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ApplicationSetup();

            ViewModelApplication.GoToPage(ApplicationPage.WorkoutLog);

            Current.MainWindow = new MainWindow();
            var windowViewModel = new WindowViewModel(Current.MainWindow);


            using var db = new AppDbContext();

            var _top = db.AppSettings.FirstOrDefault(x => x.SettingName == "WindowTop");
            double.TryParse(_top.Value ?? _top.DefaultValue, out var top);
            var _left = db.AppSettings.FirstOrDefault(x => x.SettingName == "WindowLeft");
            double.TryParse(_left.Value ?? _left.DefaultValue, out var left);
            var _height = db.AppSettings.FirstOrDefault(x => x.SettingName == "WindowHeight");
            double.TryParse(_height.Value ?? _height.DefaultValue, out var height);
            var _width = db.AppSettings.FirstOrDefault(x => x.SettingName == "WindowWidth");
            double.TryParse(_width.Value ?? _width.DefaultValue, out var width);

            windowViewModel.SetWindowSizeAndPosition(top, left, height, width);
            Current.MainWindow.DataContext = windowViewModel;

            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            //_ = new AppDbContext();

            Framework.Construct<DefaultFrameworkConstruction>()
            .AddFileLogger()
            .AddAppViewModels()
            .Build();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: Application_DispatcherUnhandledException", args.Exception.ToString()));

            args.Handled = true;
        }

        private void TaskSchedulerOnUnobservedTaskException(UnobservedTaskExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: TaskSchedulerOnUnobservedTaskException", args.Exception.ToString()));
        }

        private void CurrentOnDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: CurrentOnDispatcherUnhandledException", args.Exception.ToString()));

            args.Handled = true;
        }
    }
}