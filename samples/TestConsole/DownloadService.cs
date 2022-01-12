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
        await _service.Get("https://garywoodfine.com", default);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}