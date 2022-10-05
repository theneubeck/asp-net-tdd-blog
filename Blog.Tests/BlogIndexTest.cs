using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Tests.Setup;
using Xunit;

namespace Blog.Tests;

public class BlogTest : IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private HttpClient _client;

    public BlogTest(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task EmptyBlogIndexTest()
    {
        var response = await _client.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        // var body = await response.Content.ReadFromJsonAsync<BlogListReponse>();
        var expected = Utils.ToJson(new { Posts = new List<object>() });
        Assert.Equal(expected, responseBody);
    }


    [Fact]
    public async Task BlogPostNotFoundTest()
    {
        var response = await _client.GetAsync("/posts/b4576c30-4e2b-4ace-adee-a683e417b706");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task BlogPostMalformedTest()
    {
        var response = await _client.GetAsync("/posts/invalid");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
