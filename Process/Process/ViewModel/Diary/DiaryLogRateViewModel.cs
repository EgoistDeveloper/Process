using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Process.Data;
using Process.Models.Diary;

namespace Process.ViewModel.Diary
{
    public class DiaryLogRateViewModel : WindowViewModel
    {
        public DiaryLogRateViewModel(Window window, DiaryLog diaryLog) : base(window)
        {
            mWindow = window;
            WindowMinimumWidth = 900;
            WindowMinimumHeight = 550;

            DiaryLog = diaryLog ?? new DiaryLog();
            DiaryLog.DiaryLogRate ??= new DiaryLogRate();

            Title = $"Rate Your Day: {diaryLog.Date}";

            CloseCommand = new RelayCommand(p => CloseWindow());

            LoadFeelings();
        }

        #region Properties

        /// <summary>
        /// Current diary log
        /// </summary>
        public DiaryLog DiaryLog { get; set; } = new DiaryLog();
        public List<DiaryLogFeeling> DiaryLogFeelings { get; set; }

        #endregion

        #region Methods

        public void LoadFeelings()
        {
            DiaryLogFeelings = new List<DiaryLogFeeling>
            {
                new DiaryLogFeeling()
                {
                    Feeling = "Happy"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Alive"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Good"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Love"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Interested"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Positive"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Strong"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Angry"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Depressed"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Confused"
                },
                new DiaryLogFeeling()
                {
                    Feeling = "Helpless"
                }
            };
        }

        public void LoadLogRate()
        {
            using var db = new AppDbContext();
        }

        public void SaveRate()
        {
            if (DiaryLogFeelings.Count <= 0) return;

            var feelings = new List<DiaryLogFeeling>();

            foreach (var feeling in DiaryLogFeelings)
            {
                 if (feeling.FeelingRate > 0)
                 {
                     feelings.Add(feeling);
                 }
            }

            if (feelings.Count > 0)
            {
                DiaryLog.DiaryLogRate.DiaryLogFeelings = feelings;
                DiaryLog.DiaryLogRate.DiaryLogId = DiaryLog.Id;

                using var db = new AppDbContext();
                db.DiaryLogRates.Add(DiaryLog.DiaryLogRate);
                db.SaveChanges();
            }
        }

        public void UpdateRate()
        {
            using var db = new AppDbContext();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Close current window
        /// </summary>
        public void CloseWindow()
        {
            if (DiaryLog.DiaryLogRate != null && DiaryLog.DiaryLogRate.Id > 0)
            {
                UpdateRate();
            }
            else
            {
                SaveRate();
            }

            mWindow.Close();
        }

        #endregion
    }
}
