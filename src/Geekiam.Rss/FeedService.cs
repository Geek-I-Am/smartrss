using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
[assembly: InternalsVisibleTo("Geekiam.Rss.Tests")]
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

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return  ParseFeedUrlsFromHtml(System.Net.WebUtility.HtmlDecode(content));
    }


    private IEnumerable<FeedLink> ParseFeedUrlsFromHtml(string htmlContent)
    {
      

        var rex = new Regex("<link[^>]*rel=\"alternate\"[^>]*>", RegexOptions.Singleline);

        var result = new List<FeedLink>();

        foreach (Match m in rex.Matches(htmlContent))
        {
            var hfl = ExtractLink(m.Value);
            if (hfl != null)
                result.Add(hfl);
        }

        return result.AsEnumerable();
    }

    internal FeedLink ExtractLink(string content)
    {
       var type = AttributeFromTag("type", content).ToLower();

        if (!type.Contains("application/rss") && !type.Contains("application/atom"))
            return null;

       
        return new FeedLink( AttributeFromTag("title", content),AttributeFromTag("href", content) , type.Contains("rss") ? FeedType.Rss : FeedType.Atom);
    }

    private string AttributeFromTag(string attribute, string htmlTag)
    {
        var res = Regex.Match(htmlTag, attribute + "\\s*=\\s*\"(?<val>[^\"]*)\"", RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);

        return res.Groups.Count > 1 ? res.Groups[1].Value : string.Empty;
    }
}