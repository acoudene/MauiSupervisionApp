// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using CommunityToolkit.Diagnostics;
using MauiSupervisionApp.Exceptions.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;

namespace MauiSupervisionApp.Exceptions.Helpers;

/// <summary>
/// Extension methods for setting up exceptions services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ExceptionManagementServiceCollectionExtensions
{
  /// <summary>
  /// Adds services required for application localization.
  /// </summary>
  /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
  /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
  public static IServiceCollection AddExceptionManagement(this IServiceCollection services)
  {
    Guard.IsNotNull(services);

    services.TryAdd(services.AddLocalization());
    services.TryAdd(services.AddMudServices());

    services.AddScoped<ILogService, SnackbarService>();


    return services;
  }
}