using Process.Data;
using System.Collections.Generic;

namespace Process.Models.Diary
{
    public class MonthFactory : IMonth
    {
        private DiaryMonth _month { get; set; }

        public MonthFactory(DiaryMonth month)
        {
            _month = month;
        }

        public DiaryMonth CreateMonth()
        {
            return _month;
        }

        public IMonth SetDays(List<DiaryDay> days)
        {
            _month.DiaryDays = days;

            return this;
        }

        public IMonth SetMonthName(int monthnumber)
        {
            _month.MonthName = Settings.CultureInfo.DateTimeFormat.GetMonthName(monthnumber);

            return this;
        }

        public IMonth SetMonthNumber(int monthNumber)
        {
            _month.MonthNumber = monthNumber;

            return this;
        }

        public IMonth SetYear(int year)
        {
            _month.Year = year;

            return this;
        }
    }

    public static class FluentMonthFactory
    {
        public static IMonth Month()
        {
            return new MonthFactory(new DiaryMonth());
        }
    }

}
