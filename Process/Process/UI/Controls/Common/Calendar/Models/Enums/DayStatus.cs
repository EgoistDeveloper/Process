namespace Process.UI.Controls.Common.Calendar.Models
{
    public enum DayStatus
    {
        DayOfPreviousMonth = 0, // day of previous month on calendar
        DayOfNextMonth = 1,     // day of next month on calendar
        EmptyDay = 2,           // empty day, not logged day
        EmptyToday = 3,         // current empty day
        LoggedDay = 4,          // logged day
        LoggedToday = 5         // logged today
    }
}
