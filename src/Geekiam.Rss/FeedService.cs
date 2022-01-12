using System.Text;
using System.Text.RegularExpressions;

namespace Geekiam.Rss;

public class FeedService : IFeedService
{
    private readonly HttpClient _client;

    public FeedService(HttpClient client)
    {
        _client = client;
    }


    public async Task<IEnumerable<FeedLink>> Get(string url, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new Exception();

        var pageContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return  ParseFeedUrlsFromHtml(pageContent);
    }
    
   
    
    public IEnumerable<FeedLink> ParseFeedUrlsFromHtml(string htmlContent)
    {
        // sample link:
        // <link rel="alternate" type="application/rss+xml" title="Microsoft Bot Framework Blog" href="http://blog.botframework.com/feed.xml">
        // <link rel="alternate" type="application/atom+xml" title="Aktuelle News von heise online" href="https://www.heise.de/newsticker/heise-atom.xml">

        Regex rex = new Regex("<link[^>]*rel=\"alternate\"[^>]*>", RegexOptions.Singleline);

        List<FeedLink> result = new List<FeedLink>();

        foreach (Match m in rex.Matches(htmlContent))
        {
            var hfl = GetLinks(m.Value);
            if (hfl != null)
                result.Add(hfl);
        }

        return result.AsEnumerable();
    }

    internal  FeedLink GetLinks(string content)
    {
        string linkTag = System.Net.WebUtility.HtmlDecode(content);
        string type = GetAttributeFromLinkTag("type", linkTag).ToLower();

        if (!type.Contains("application/rss") && !type.Contains("application/atom"))
            return null;

        FeedLink hfl = new FeedLink();
        string title = GetAttributeFromLinkTag("title", linkTag);
        string href = GetAttributeFromLinkTag("href", linkTag);
        hfl.Title = title;
        hfl.Url = href;
        hfl.Type = type.Contains("rss") ? FeedType.Rss : FeedType.Atom;
        return hfl;
    }
    private static string GetAttributeFromLinkTag(string attribute, string htmlTag)
    {
        var res = Regex.Match(htmlTag, attribute + "\\s*=\\s*\"(?<val>[^\"]*)\"", RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);

        return res.Groups.Count > 1 ? res.Groups[1].Value : string.Empty;
    }
}