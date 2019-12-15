using Process.Models.Diary;
using System.Windows;
using System.Windows.Input;

namespace Process.ViewModel.Diary
{
    /// <summary>
    /// The View Model for the DiaryLogPreview window
    /// </summary>
    public class DiaryLogPreviewViewModel : WindowViewModel
    {
        /// <summary>
        /// Current constructor
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="diaryHistoryLog">DiaryHistoryLog</param>
        public DiaryLogPreviewViewModel(Window window, DiaryHistoryLog diaryHistoryLog) : base(window)
        {
            mWindow = window;

            DiaryHistoryLog = diaryHistoryLog;
            Title = diaryHistoryLog.AddedDate.ToString();

            OpenHyperlinkCommand = new RelayParameterizedCommand(OpenHyperlink);
        }

        #region Commands

        /// <summary>
        /// Open hyper link command
        /// </summary>
        public ICommand OpenHyperlinkCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Current history log
        /// </summary>
        public DiaryHistoryLog DiaryHistoryLog { get; set; } = new DiaryHistoryLog();

        #endregion

        #region Methods

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
