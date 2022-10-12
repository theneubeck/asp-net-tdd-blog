using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Tests.Models;
using Blog.Tests.Setup;
using FluentAssertions;
using Xunit;

namespace Blog.Tests;

public class BlogTest : IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BlogTest(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        _factory.DbContext.SaveChanges();
    }


    [Fact]
    public async Task ListEmptyBlogPostListTest()
    {
        
        var response = await _client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await response.Content.ReadAsStringAsync();
        var expected = Utils.ToJson(new { Posts = new List<object>() });
        responseBody.Should().Be(expected);
    }
    
}
