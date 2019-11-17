using System;
using System.Collections.Generic;

namespace Process.Models.Diary
{
    public interface IDiaryLog
    {
        DiaryLog CreateDiaryLog();
        IDiaryLog SetId(long id);
        IDiaryLog SetDate(DateTime dateTime);
        IDiaryLog SetAddedDate(DateTime dateTime);
        IDiaryLog SetUpdateDate(DateTime dateTime);
        IDiaryLog SetLogContent(string log);
        IDiaryLog SetLogContentLength(long contentLength);

        IDiaryLog SetHistoryLogs(List<DiaryHistoryLog> historyLogs);
        IDiaryLog AddHistoryLog(DiaryHistoryLog historyLog);
    }
}
