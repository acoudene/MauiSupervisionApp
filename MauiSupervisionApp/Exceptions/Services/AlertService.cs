// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using CommunityToolkit.Diagnostics;
using MauiSupervisionApp.Exceptions.Base;
using MauiSupervisionApp.Exceptions.Models;
using MauiSupervisionApp.Exceptions.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace MauiSupervisionApp.Exceptions.Services;

public class AlertService : ILogService
{  
  private readonly IStringLocalizer<LogResource> _stringLocalizer;

  public AlertService(IStringLocalizer<LogResource> stringLocalizer)
  {
    Guard.IsNotNull(stringLocalizer);

    _stringLocalizer = stringLocalizer;
  }

  public LogVO GenerateLog(Exception exception)
  {
    Guard.IsNotNull(exception);

    var log = default(LogVO);

    var loggedException = exception as ILoggedException;
    if (loggedException != null)
    {
      log = loggedException.Log;
    }

    log = log ?? new LogVO()
    {
      Message = exception.Message,
      Exception = exception
    };

    return log;
  }

  public string GetStringMessageContent(Exception exception) => GetStringMessageContent(GenerateLog(exception));

  public string GetStringMessageContent(LogVO log)
  {
    Guard.IsNotNull(log);

    string categoryName = log.CategoryName ?? nameof(Exception);

    string title = log.Message ?? categoryName;
    string innerCategoryName = log.InnerCategoryName ?? string.Empty;

    string id = log.Id.ToString();

    // Set dedicated message following to environment
    bool isDevelopment = true;
    string message = !isDevelopment
    ?
        $"{title}\n" +
        $"{_stringLocalizer[categoryName]}\n" +
        $"<li>{_stringLocalizer["Category"]}: {categoryName}\n" +
        $"{_stringLocalizer["Identifier"]}: {id}\n" +
        $"{_stringLocalizer["Contact support"]}\n"
    :
        $"{title}\n" +
        $"{_stringLocalizer[categoryName]}\n" +
        $"{_stringLocalizer["Category"]}: {categoryName}\n" +
        $"{_stringLocalizer["InnerCategory"]}: {innerCategoryName}\n" +
        $"{_stringLocalizer["Identifier"]}: {id}\n" +
        $"{string.Join("|", log.ExceptionMessages)}\n" + // Dev
        $"{_stringLocalizer["See logs"]} / {_stringLocalizer["Contact support"]}\n" // Dev
    ;
    return message;
  }

  public string GetRawHtmlMessageContent(Exception exception) => GetRawHtmlMessageContent(GenerateLog(exception));

  public string GetRawHtmlMessageContent(LogVO log)
  {
    Guard.IsNotNull(log);

    string categoryName = log.CategoryName ?? nameof(Exception);

    string title = log.Message ?? categoryName;
    string innerCategoryName = log.InnerCategoryName ?? string.Empty;

    string id = log.Id.ToString();

    // Set dedicated message following to environment
    bool isDevelopment = true;
    string message = !isDevelopment
    ?
        $"<ul>" +
        $"<li>{title}</li>" +
        $"<li>{_stringLocalizer[categoryName]}</li>" +
        $"<li>{_stringLocalizer["Category"]}: {categoryName}</li>" +
        $"<li>{_stringLocalizer["Identifier"]}: {id}</li>" +
        $"<li>{_stringLocalizer["Contact support"]}</li>" +
        $"</ul>"
    :
        $"<ul>" +
        $"<li>{title}</li>" +
        $"<li>{_stringLocalizer[categoryName]}</li>" +
        $"<li>{_stringLocalizer["Category"]}: {categoryName}</li>" +
        $"<li>{_stringLocalizer["InnerCategory"]}: {innerCategoryName}</li>" +
        $"<li>{_stringLocalizer["Identifier"]}: {id}</li>" +
        $"<li>{string.Join("|", log.ExceptionMessages)}</li>" + // Dev
        $"<li>{_stringLocalizer["See logs"]} / {_stringLocalizer["Contact support"]}</li>" + // Dev
        $"</ul>";

    return message;
  }

  public Task<LogVO> NotifyAsync(Exception exception) => NotifyAsync(GenerateLog(exception));

  public async Task<LogVO> NotifyAsync(LogVO log)
  {
    Guard.IsNotNull(log);

    string message = GetStringMessageContent(log);

    // Create the notification with error options
    var mainPage = Application.Current?.MainPage;
    if (mainPage == null)
      return log;

    await mainPage.DisplayAlert(_stringLocalizer["Error"], message, _stringLocalizer["Ok"]);

    return log;
  }
}
