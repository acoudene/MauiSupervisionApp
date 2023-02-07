// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Services;

public interface ILogService
{
  LogVO GenerateLog(Exception exception);

  string GetMessageContent(LogVO log);
  string GetMessageContent(Exception exception);

  LogVO Notify(LogVO log);
  LogVO Notify(Exception exception);
}
