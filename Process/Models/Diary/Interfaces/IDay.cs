using Process.Models.Common;


namespace Process.Models.Diary
{
    public interface IDay
    {
        DiaryDay CreateDay();
        IDay SetDateFrom(int year, int month, int day);
        IDay IsEnabled(bool isEnabled);
        IDay IsDefault(bool isDefault);
        IDay SetTooltip(string tooltipText);
        IDay SetDiaryLog(DiaryLog diaryLog);
        IDay SetDayStatus(DayStatus dayStatus);
    }
}
