using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Blog.Tests;

public class BlogTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public BlogTest(CustomWebApplicationFactory<Program> factory)
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
        var expected = Utils.ToJson(new { Posts = new List<BlogPostReponse>() });
        Assert.Equal(expected, responseBody);
    }


    [Fact]
    public async Task BlogIndexTest()
    {
        var postBody = Utils.ToJsonStringContent(new { Title = "Title", Body = "Body" });
        var response = await _client.PostAsync("/posts", postBody);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var parsed = await response.Content.ReadFromJsonAsync<BlogPostReponse>();
        var responseBody = await response.Content.ReadAsStringAsync();

        var expected = Utils.ToJson(new { Id = parsed.Id, Title = "Title", Body = "Body" });
        Assert.Equal(expected, responseBody);
    }
}

internal class BlogListReponse
{
    public List<BlogPostReponse> Posts { get; set; }
}

public class BlogPostReponse
{
    public string Title { get; set; }
    public string Type { get; set; }

    public System.Guid Id { get; set; }
    // public string Body { get; set; }
}
