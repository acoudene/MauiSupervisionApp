// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using CommunityToolkit.Diagnostics;
using MauiSupervisionApp.Exceptions.Services;
using Microsoft.AspNetCore.Components;

namespace MauiSupervisionApp.Exceptions.Pages;

/// <summary>
/// Code-behind to manage specifically all error/exceptions
/// </summary>
public partial class CustomErrorBoundary
{
  private readonly List<Exception> _reentrantExceptions = new();

  [Inject]
  private ILogService? LogService { get; set; }

  protected string GetRawHtmlMessageContent(Exception exception)
  {
    Guard.IsNotNull(exception);
    Guard.IsNotNull(LogService);

    return LogService.GetRawHtmlMessageContent(exception);
  }

  protected override async Task OnErrorAsync(Exception exception)
  {
    Guard.IsNotNull(exception);
    Guard.IsNotNull(LogService);

    var log = await LogService.NotifyAsync(exception);
    await base.OnErrorAsync(exception);
  }

  public void Reset()
  {
    _reentrantExceptions.Clear();
    base.Recover();
  }
}