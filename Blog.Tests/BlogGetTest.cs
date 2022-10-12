using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Tests.Setup;
using FluentAssertions;
using Xunit;

namespace Blog.Tests;

public class BlogGetTest : IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BlogGetTest(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _factory.DbContext.Posts.RemoveRange(_factory.DbContext.Posts);
        _factory.DbContext.SaveChanges();

    }
    
    [Fact]
    public async Task ForNonExistingIdShouldReturn404()
    {
        var response = await _client.GetAsync($"/posts/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ForInvalidIdShouldReturn400()
    {
        var response = await _client.GetAsync($"/posts/invalid");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
