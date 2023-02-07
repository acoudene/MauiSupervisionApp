// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using CommunityToolkit.Diagnostics;
using MauiSupervisionApp.Exceptions.Base;
using MauiSupervisionApp.Exceptions.Models;
using MauiSupervisionApp.Exceptions.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace MauiSupervisionApp.Exceptions.Services;

public class SnackbarService : ILogService
{
  private readonly ISnackbar _snackbar;
  private readonly IStringLocalizer<LogResource> _stringLocalizer;

  public SnackbarService(ISnackbar snackbar, IStringLocalizer<LogResource> stringLocalizer)
  {
    Guard.IsNotNull(snackbar);
    Guard.IsNotNull(stringLocalizer);

    _snackbar = snackbar;
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

  public string GetMessageContent(Exception exception) => GetMessageContent(GenerateLog(exception));

  public string GetMessageContent(LogVO log)
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

  public LogVO Notify(Exception exception) => Notify(GenerateLog(exception));

  public LogVO Notify(LogVO log)
  {
    Guard.IsNotNull(log);

    string message = GetMessageContent(log);

    // Create the notification with error options
    _snackbar.Add(message, Severity.Error, options =>
    {
      options.ShowCloseIcon = true;
      options.VisibleStateDuration = 60000;
      options.HideTransitionDuration = 500;
      options.ShowTransitionDuration = 500;
      options.SnackbarVariant = Variant.Filled;
    });

    return log;
  }
}
