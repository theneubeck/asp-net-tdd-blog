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

public class BlogCreationTest : IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BlogCreationTest(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        // TO clean db between runs, uncomment this
        // _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        // _factory.DbContext.SaveChanges();
    }


    [Fact]
    public async Task CreateBlogPostTest()
    {
        var response = await _client.PostAsJsonAsync("/posts", new         { Title = "Title", Body = "Body" }); 
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await response.Content.ReadFromJsonAsync<TestBlogPostModel>();
        var expected = new TestBlogPostModel {Title = "Title", Body = "Body"};
        responseBody.Should().BeEquivalentTo(expected);
    }
}
