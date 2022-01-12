namespace Geekiam.Rss;

public class FeedLink
{
    public string Title { get; set; }
    public string Url { get; set; }
    public FeedType Type { get; set; }

    public FeedLink()
    {
        
    }

    public FeedLink(string title, string url, FeedType type )
    {
        Title = title;
        Url = url;
        Type = type;

    }
    
}