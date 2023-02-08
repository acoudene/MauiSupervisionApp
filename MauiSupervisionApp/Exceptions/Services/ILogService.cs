// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Services;

public interface ILogService
{
  LogVO GenerateLog(Exception exception);

  string GetStringMessageContent(LogVO log);
  string GetStringMessageContent(Exception exception);

  string GetRawHtmlMessageContent(LogVO log);
  string GetRawHtmlMessageContent(Exception exception);

  Task<LogVO> NotifyAsync(LogVO log);
  Task<LogVO> NotifyAsync(Exception exception);
}
