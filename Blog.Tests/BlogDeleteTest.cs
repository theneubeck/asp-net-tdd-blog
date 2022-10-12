using System;
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

public class BlogDeleteTest : IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BlogDeleteTest(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        _factory.DbContext.SaveChanges();

    }


    [Fact]
    public async Task DeleteBlogForExistingBlogShouldReturnNoContent()
    {
        var createdResponse = await _client.PostAsJsonAsync("/posts", new BlogPostModel
        {
            Title = "some title",
            Body = "very valid body fff",
        });

        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdPost = await createdResponse.Content.ReadFromJsonAsync<BlogPostModel>();
        
        var response = await _client.DeleteAsync($"/posts/{createdPost.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/posts/{createdPost.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteBlogForNoneExistingBlogTest()
    {
        var response = await _client.DeleteAsync($"/posts/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}
