using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Models.Diary
{
    public interface IMonth
    {
        DiaryMonth CreateMonth();
        IMonth SetDays(List<DiaryDay> days);
        IMonth SetMonthNumber(int monthNumber);
        IMonth SetMonthName(int monthnumber);
        IMonth SetYear(int year);
    }
}
