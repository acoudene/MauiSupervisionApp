using CommunityToolkit.Diagnostics;
using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Proxies
{
  public class LogClient : ILogClient
  {
    private readonly IHttpClientFactory _clientFactory;

    public LogClient(IHttpClientFactory clientFactory)
    {
      Guard.IsNotNull(clientFactory);

      _clientFactory = clientFactory;
    }

    public Task ReportAsync(LogVO log)
    {
      // To implement if needed to report to server
      throw new NotImplementedException();
      //try
      //{
      //  CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);
      //  var httpClient = _clientFactory.CreateClient();
      //  await httpClient.PostObjectAsync<LogVO>(log, "Log", cancellationTokenSource.Token);
      //}
      //catch (Exception)
      //{
      //  // Do nothing for the moment
      //}
    }
  }
}
