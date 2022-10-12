using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Api.Models;
using Blog.Tests.Models;
using Blog.Tests.Setup;
using FluentAssertions;
using Xunit;

namespace Blog.Tests;

public class BlogPostEditTests: IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BlogPostEditTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        _factory.DbContext.SaveChanges();
    }
    
    [Fact]
    public async Task EditBlogPost_Should_Update_BlogPost()
    {
        var createResult = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest {Body = "fdsfsdfsdfsdf", Title = "fsfesfsdfsdfsdf"});
        createResult.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await createResult.Content.ReadFromJsonAsync<BlogPostModel>();
        var updateRes = await _client.PutAsJsonAsync($"/posts/{content.Id}", new {Title = "new-title-sfsfjdhjasfjkfsdakf", Body = "foofoofoofoofoofoofoofoofoofoo"});
        updateRes.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var updatedPost = await _client.GetFromJsonAsync<BlogPostModel>($"/posts/{content.Id}");
        updatedPost.Body.Should().Be("foofoofoofoofoofoofoofoofoofoo");
        updatedPost.Title.Should().Be("new-title-sfsfjdhjasfjkfsdakf");
    }
    
    [Fact]
    public async Task EditBlogPost_Should_Not_Update_BlogPost_On_Invalid_Title()
    {
        var createResult = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest {Body = "valid body that is", Title = "fsfesfsdfsdfsdf"});
        createResult.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await createResult.Content.ReadFromJsonAsync<BlogPostModel>();
        var updateRes = await _client.PutAsJsonAsync($"/posts/{content.Id}", new {Body = "foofoofoofoofoofoofoofoofoofoo"});
        updateRes.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var updatedPost = await _client.GetFromJsonAsync<BlogPostModel>($"/posts/{content.Id}");
        updatedPost.Body.Should().Be("valid body that is");
        updatedPost.Title.Should().Be("fsfesfsdfsdfsdf");
    }
    
    [Fact]
    public async Task EditBlogPost_Should_Not_Update_BlogPost_On_Invalid_Body()
    {
        var createResult = await _client.PostAsJsonAsync("/posts", new BlogPostCreateRequest {Body = "valid body that is", Title = "fsfesfsdfsdfsdf"});
        createResult.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await createResult.Content.ReadFromJsonAsync<BlogPostModel>();
        var updateRes = await _client.PutAsJsonAsync($"/posts/{content.Id}", new {Title = "title that is", Body = "foo"});
        updateRes.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var updatedPost = await _client.GetFromJsonAsync<BlogPostModel>($"/posts/{content.Id}");
        updatedPost.Body.Should().Be("valid body that is");
        updatedPost.Title.Should().Be("fsfesfsdfsdfsdf");
    }
    
    [Fact]
    public async Task EditBlogPost_Should_Give_404_On_None_Existing()
    {
        var updateRes = await _client.PutAsJsonAsync($"/posts/{Guid.NewGuid()}", new {Title = "title that is", Body = "foo is foo but a bar"});
        updateRes.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}
