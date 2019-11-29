using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Process.Data;
using Process.Models.Diary;
using Process.Dialogs.Diary;

namespace Process.ViewModel.Diary
{
    /// <summary>
    /// The View Model for the DiaryLogInput window
    /// </summary>
    public class AddDiaryLogViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="diaryLog">Day.DiaryLog</param>
        /// <param name="date">Day.Date</param>
        public AddDiaryLogViewModel(Window window, DateTime date, DiaryLog diaryLog = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 600;
            WindowMinimumWidth = 1100;

            DiaryLog = diaryLog != null ? diaryLog : new DiaryLog();
            DiaryLog.Date = date != null ? date : DateTime.Now;
            Title = $"Day: {DiaryLog.Date.ToShortDateString()}";

            CloseCommand = new RelayCommand(p => AddOrUpdate());
            ShowHistoryLogCommand = new RelayParameterizedCommand(ShowHistoryLog);
            OpenHyperlinkCommand = new RelayParameterizedCommand(OpenHyperlink);
            OpenDayRateCommand = new RelayCommand(p => OpenDayRate());

            LoadLog();
        }

        #region Commands

        public ICommand OpenHyperlinkCommand { get; set; }
        public ICommand OpenDayRateCommand { get; set; }
        public ICommand ShowHistoryLogCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Current diary log
        /// </summary>
        public DiaryLog DiaryLog { get; set; }

        /// <summary>
        /// Log content clone for comparing updated content
        /// </summary>
        public string DiaryLogContentClone { get; set; }


        #endregion

        #region Methods

        /// <summary>
        /// Fetch Diary Log content
        /// </summary>
        private void LoadLog()
        {
            if (DiaryLog.Id > 0)
            {
                using var db = new AppDbContext();
                DiaryLog = db.DiaryLogs.Where(x => x.Id == DiaryLog.Id).Select(x => new DiaryLog
                {
                    Id = x.Id,
                    Date = x.Date,
                    AddedDate = x.AddedDate,
                    UpdateDate = x.UpdateDate,
                    LogContent = x.LogContent,
                    LogContentLength = x.LogContentLength,
                    DiaryHistoryLogs = x.DiaryHistoryLogs.OrderByDescending(c => c.Id).ToList()
                }).FirstOrDefault();

                if (DiaryLog != null) DiaryLogContentClone = DiaryLog.LogContent;
            }
        }

        /// <summary>
        /// Add first diary log
        /// </summary>
        private void SaveLog()
        {
            if (string.IsNullOrWhiteSpace(DiaryLog.LogContent)) return;

            using var db = new AppDbContext();
            var date = DateTime.Now;

            DiaryLog.AddedDate = date;
            DiaryLog.UpdateDate = date;
            DiaryLog.LogContentLength = DiaryLog.LogContent.Length;
            DiaryLog.DiaryHistoryLogs.Add(new DiaryHistoryLog()
            {
                DiaryLogId = DiaryLog.Id,
                AddedDate = date,
                LogContent = DiaryLog.LogContent
            });

            db.DiaryLogs.Add(DiaryLog);
            db.SaveChanges();
        }

        /// <summary>
        /// Update current log content
        /// </summary>
        private void UpdateLog()
        {
            if (DiaryLog.LogContent != DiaryLogContentClone)
            {
                using var db = new AppDbContext();
                var date = DateTime.Now;

                var diaryLog = db.DiaryLogs.Where(x => x.Id == DiaryLog.Id).Select(x => new DiaryLog
                {
                    Id = x.Id,
                    Date = x.Date,
                    AddedDate = x.AddedDate,
                    UpdateDate = x.UpdateDate,
                    LogContent = x.LogContent,
                    DiaryHistoryLogs = x.DiaryHistoryLogs.ToList()
                }).SingleOrDefault();

                if (diaryLog != null)
                {
                    diaryLog.UpdateDate = date;
                    diaryLog.LogContent = DiaryLog.LogContent;
                    diaryLog.LogContentLength = DiaryLog.LogContent.Length;
                    diaryLog.DiaryHistoryLogs.Add(new DiaryHistoryLog()
                    {
                        DiaryLogId = DiaryLog.Id,
                        AddedDate = date,
                        LogContent = DiaryLog.LogContent
                    });

                    db.DiaryLogs.Update(diaryLog);

                    db.SaveChanges();

                    DiaryLog = diaryLog;
                }
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Close current window
        /// </summary>
        private void AddOrUpdate()
        {
            if (DiaryLog.Id > 0)
            {
                UpdateLog();
            }
            else if (DiaryLog.LogContent != null && !string.IsNullOrWhiteSpace(DiaryLog.LogContent))
            {
                SaveLog();

                OpenDayRate();
            }

            mWindow.Close();
        }

        /// <summary>
        /// Show selected diary history log content for preview
        /// </summary>
        /// <param name="sender"></param>
        public void ShowHistoryLog(object sender)
        {
            if (sender == null || !(sender is Button button)) return;
            if (!(button.DataContext is DiaryHistoryLog historyLog)) return;

            var dialog = new PreviewDiaryLogDialog();
            dialog.ShowDialogWindow(new DiaryLogPreviewViewModel(dialog, historyLog), mWindow);
        }

        /// <summary>
        /// Open hyper link
        /// </summary>
        /// <param name="link"></param>
        public void OpenHyperlink(object link)
        {
            if (!(link is string input)) return;
            System.Diagnostics.Process.Start(input);
        }

        /// <summary>
        /// Open day rate dialog
        /// </summary>
        public void OpenDayRate()
        {
            var dialog = new DiaryLogRatingDialog();

            dialog.Closing += (s, e) =>
            {
                if (dialog.DataContext is DiaryLogRateViewModel vm && vm.DiaryLog != null)
                {
                    DiaryLog = vm.DiaryLog;
                }
            };

            dialog.ShowDialogWindow(new DiaryLogRateViewModel(dialog, DiaryLog), mWindow);
        }

        #endregion
    }
}
