using Shouldly;
using Xunit;

namespace Geekiam.Rss.Tests;

public class ReaderTests
{
    [Theory]
    [InlineData("garywoodfine.com", "http://garywoodfine.com:80/")]
    [InlineData("https://garywoodfine.com", "https://garywoodfine.com:443/")]
    public void ShouldReturnAbsoluteUrl(string url, string expected)
    {
      
      

    }
}