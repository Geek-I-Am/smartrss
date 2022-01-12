namespace Geekiam.Rss;

public class FeedExplorer
{
    private readonly HttpClient _client;

    public FeedExplorer(HttpClient client)
    {
        _client = client;
    }


    public async Task<IList<FeedLink>> GetFeeds(string url, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new Exception();

        var pageContent = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await GetLinks(pageContent);
    }

    internal async Task<IList<FeedLink>> GetLinks(Stream stream)
    {
        return null;
    }
}