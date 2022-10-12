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
        _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        _factory.DbContext.SaveChanges();
    }


    [Fact]
    public async Task CreateBlogPostTest()
    {
        var response = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest
        {
            Title = "Title",
            Body = "Body is allowed"
        });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await response.Content.ReadFromJsonAsync<BlogPostModel>();
        var expected = new BlogPostModel {Title = "Title", Body = "Body is allowed", Id = responseBody!.Id};
        responseBody.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task CreateBlogWithEmptyTitleReturns400()
    {
        
        var response = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest
        {
            Title = "",
            Body = "Bodygdgdgdgdgd"
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var listResponse = await _client.GetFromJsonAsync<BlogPostListModel>("/");
        listResponse.Posts.Should().BeEmpty();

    }
    
    [Fact]
    public async Task CreateBlogWithBodyShorterThan10CharsReturns400()
    {
        
        var response = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest
        {
            Title = "",
            Body = "11111111"
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var listResponse = await _client.GetFromJsonAsync<BlogPostListModel>("/");
        listResponse.Posts.Should().BeEmpty();
    }
}
