using Geekiam.Rss;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestConsole;

class Program
{
    private const string EnvironmentVariablePrefix = "GEEKIAM_FEED_";
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder().ConfigureHostConfiguration(
            config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
               // config.AddJsonFile("hostsettings.json", true);
                config.AddEnvironmentVariables(EnvironmentVariablePrefix);
                config.AddCommandLine(args);
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient<IFeedService, FeedService>(client =>
                {
                    client.DefaultRequestHeaders.Add(GeekiamFeed.ACCEPT_HEADER_NAME, GeekiamFeed.ACCEPT_HEADER_VALUE);
                    client.DefaultRequestHeaders.Add(GeekiamFeed.USER_AGENT_NAME, GeekiamFeed.USER_AGENT_VALUE);
                    client.DefaultRequestHeaders.Add(GeekiamFeed.REFERER_HEADER_NAME, GeekiamFeed.REFERER_HEADER_VALUE);
                });
                services.AddHostedService<DownloadService>();
            }).Build();

        await host.RunAsync();
    }
}