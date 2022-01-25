using Geekiam.Rss;
using Microsoft.Extensions.Hosting;

namespace TestConsole;

public class DownloadService : IHostedService
{
    private readonly IFeedService _service;

    public DownloadService(IFeedService service)
    {
        _service = service;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var feeds = await _service.Get("https://cryptobriefing.com/", default);
        feeds.ToList().ForEach(feed => Console.Write($"{feed.Title}  {feed.Type} {feed.Url}\n"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}