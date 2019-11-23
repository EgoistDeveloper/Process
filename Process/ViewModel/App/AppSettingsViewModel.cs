using GalaSoft.MvvmLight;
using Process.Data;
using Process.Helpers;
using Process.Models.AppSetting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static Process.DI.DI;

namespace Process.ViewModel.App
{
    public class AppSettingsViewModel : ViewModelBase
    {
        public AppSettingsViewModel()
        {
            SaveSettingCommand = new RelayParameterizedCommand(SaveSetting);

            using var db = new AppDbContext();
            AppSettings = db.AppSettings.ToObservableCollection();
        }

        public ICommand SaveSettingCommand { get; set; }

        public ObservableCollection<AppSetting> AppSettings { get; set; } = new ObservableCollection<AppSetting>();

        public void SaveSetting(object sender)
        {
            var appSetting = (sender as TextBox).DataContext as AppSetting;

            using var db = new AppDbContext();
            db.AppSettings.Update(appSetting);
            db.SaveChanges();

            ViewModelApplication.AppSettings.LoadSettings();
        }
    }
}
