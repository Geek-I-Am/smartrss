namespace Geekiam.Rss;

public interface IFeedService
{
   Task<IEnumerable<Feed>> Get(string url, CancellationToken cancellationToken);
}