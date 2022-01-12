namespace Geekiam.Rss;

public interface IFeedService
{
   Task<IEnumerable<FeedLink>> Get(string url, CancellationToken cancellationToken);
}