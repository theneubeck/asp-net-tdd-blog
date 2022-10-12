using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blog.Tests.Setup;
using Xunit;

namespace Blog.Tests;

public class BlogTest : IntegrationTestBase
{
    private readonly HttpClient _client;

    public BlogTest(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task ListBlogPostTest()
    {
        // var response = await _client.GetAsync("/");
        // Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // var responseBody = await response.Content.ReadAsStringAsync();
        // var expected = Utils.ToJson(new { Posts = new List<object>() });
        // Assert.Equal(expected, responseBody);
    }
}
