using System.Collections.Generic;
using System.Net.Http;
using Moq;
using Shouldly;
using Xunit;

namespace Geekiam.Rss.Tests;

public class FeedServiceTests
{
    private Mock<HttpClient> _mockClient = new Mock<HttpClient>();
    private FeedService _classUnderTest;

    public FeedServiceTests()
    {
        _classUnderTest = new FeedService(_mockClient.Object);
    }


    [Theory]
    [MemberData(nameof(LinkData))]
    public void ShouldParseLink2(string value1, FeedLink expected)
    {
        var result = _classUnderTest.ExtractLink(value1);

        result.ShouldBeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> LinkData =>
        new List<object[]>
        {
            new object[]
            {
                "<link rel=\"alternate\" type=\"application/rss+xml\" title=\"Microsoft Bot Framework Blog\" href=\"http://blog.botframework.com/feed.xml\">",
                new FeedLink("Microsoft Bot Framework Blog", "http://blog.botframework.com/feed.xml", FeedType.Rss)
            },
            new object[]
            {
                "<link rel=\"alternate\" type=\"application/rss+xml\" title=\"Gary Woodfine &raquo; Feed\" href=\"https://garywoodfine.com/feed/\" />",
                new FeedLink("Gary Woodfine &raquo; Feed", "https://garywoodfine.com/feed/", FeedType.Rss)
            },
        };
}