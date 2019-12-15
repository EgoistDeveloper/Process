using GalaSoft.MvvmLight;
using Process.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Process.DI.DI;

namespace Process.ViewModel.App
{
    public class RightSidebarViewModel: ViewModelBase
    {
        public RightSidebarViewModel()
        {
            GoToSettingsCommand = new RelayCommand(p => GoToSettings());
        }

        public ICommand GoToSettingsCommand { get; set; }

        public void GoToSettings()
        {
            if (ViewModelApplication.CurrentPage != ApplicationPage.AppSettings)
            {
                ViewModelApplication.GoToPage(ApplicationPage.AppSettings);
            }
        }
    }
}