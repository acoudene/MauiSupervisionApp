using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Proxies
{
  public interface ILogClient
  {
    Task ReportAsync(LogVO log);
  }
}