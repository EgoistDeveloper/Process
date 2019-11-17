using Process.Models.Common;
using System;

namespace Process.Models.Diary
{
    public class DayFactory : IDay
    {
        private DiaryDay _day { get; set; }

        public DayFactory(DiaryDay day)
        {
            _day = day;
        }

        public DiaryDay CreateDay()
        {
            return _day;
        }

        public IDay SetDateFrom(int year, int month, int day)
        {
            _day.Date = new DateTime(year, month, day);

            var date = DateTime.Now;

            if (date.Date == _day.Date.Date)
            {
                _day.TooltipText = "Today";
                _day.DayStatus = DayStatus.EmptyToday;
                _day.IsDefault = true;
            }

            return this;
        }

        public IDay IsEnabled(bool isEnabled)
        {
            _day.IsEnabled = isEnabled;

            return this;
        }

        public IDay IsDefault(bool isDefault)
        {
            _day.IsDefault = isDefault;

            return this;
        }

        public IDay SetDayStatus(DayStatus dayStatus)
        {
            _day.DayStatus = dayStatus;

            return this;
        }

        public IDay SetDiaryLog(DiaryLog diaryLog)
        {
            _day.DiaryLog = diaryLog;

            return this;
        }

        public IDay SetTooltip(string tooltipText)
        {
            if (!string.IsNullOrEmpty(tooltipText))
            {
                _day.TooltipText = tooltipText;
            }

            return this;
        }
    }

    public static class FluentDayFactory
    {
        public static IDay Day()
        {
            return new DayFactory(new DiaryDay()); ;
        }
    }
}
