using System;
using System.Windows;
using System.Windows.Input;
using Process.Data;
using Process.Models.Calendar;

namespace Process.ViewModel.Calendar
{
    public class CalendarLogViewModel : WindowViewModel
    {
        public CalendarLogViewModel(Window window, DateTime date, CalendarLog calendarLog = null) : base(window)
        {
            mWindow = window;
            WindowMinimumHeight = 600;
            WindowMinimumWidth = 1100;

            CalendarLog = calendarLog ?? new CalendarLog();
            CalendarLog.Date = date != null ? date : DateTime.Now;
            Title = $"Day: {CalendarLog.Date.ToShortDateString()}";

            CloseCommand = new RelayCommand(p => AddOrUpdate());
            OpenHyperlinkCommand = new RelayParameterizedCommand(OpenHyperlink);
        }

        #region Commands

        public ICommand OpenHyperlinkCommand { get; set; }

        #endregion

        #region Properties

        public CalendarLog CalendarLog { get; set; }

        #endregion

        #region Methods

        public void AddOrUpdate()
        {
            using var db = new AppDbContext();

            _ = !string.IsNullOrWhiteSpace(CalendarLog.Activity) && CalendarLog.Id > 0 ?
                db.CalendarLogs.Update(CalendarLog) :
                db.CalendarLogs.Add(CalendarLog);

            db.SaveChanges();

            mWindow.Close();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Open hyper link
        /// </summary>
        /// <param name="link"></param>
        public void OpenHyperlink(object link)
        {
            if (!(link is string input)) return;
            System.Diagnostics.Process.Start(input);
        }

        #endregion
    }
}
