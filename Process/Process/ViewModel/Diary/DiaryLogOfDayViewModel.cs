using GalaSoft.MvvmLight;
using Process.Data;
using Process.Dialogs.Diary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Process.ViewModel.Diary
{
    public class DiaryLogOfDayViewModel : ViewModelBase
    {
        public DiaryLogOfDayViewModel()
        {
            using var db = new AppDbContext();
            var log = db.DiaryLogs.FirstOrDefault(x => EF.Functions.Like(x.Date.ToShortDateString(), DateTime.Now.ToShortDateString()));

            if (log == null)
            {
                ControlVisibility = Visibility.Visible;
            }

            AddQuickDiaryLogkCommand = new RelayCommand(p => AddQuickDiaryLog());
        }

        #region Commands

        public ICommand AddQuickDiaryLogkCommand { get; set; }

        #endregion

        #region Properties

        public Visibility ControlVisibility { get; set; } = Visibility.Collapsed;

        #endregion

        #region Command Methods

        public void AddQuickDiaryLog()
        {
            var dialog = new DiaryLogDialog();

            dialog.Closing += (s, a) =>
            {
                if (dialog.DataContext is AddDiaryLogViewModel vm && vm.DiaryLog.LogContentLength > 0)
                {
                    ControlVisibility = Visibility.Collapsed;
                }
            };

            dialog.ShowDialogWindow(new AddDiaryLogViewModel(dialog, DateTime.Now));
        }

        #endregion
    }
}