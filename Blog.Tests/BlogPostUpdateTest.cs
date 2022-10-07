using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Tests.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests;

public class BlogPostUpdateTest : IntegrationTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;

    public BlogPostUpdateTest(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task UpdateBlogPostTest()
    {
        var postBody = Utils.ToJsonStringContent(new {Title = "Title", Body = "Some valid body"});
        var createResponse = await _client.PostAsync("/posts", postBody);
        var parsed = await createResponse.Content.ReadFromJsonAsync<BlogPostReponse>();
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, await createResponse.Content.ReadAsStringAsync());
        
        var updateBody = Utils.ToJsonStringContent(new {Title = "Title2", Body = "Some other valid body"});
        var updateResponse = await _client.PutAsync($"/posts/{parsed.Id}", updateBody);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK, await createResponse.Content.ReadAsStringAsync());

        var expected =
            Utils.ToJson(new BlogPostReponse() {Id = parsed.Id, Type = "post", Title = "Title2", Body = "Some other valid body"});
        var response = await _client.GetAsync($"/posts/{parsed.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, await response.Content.ReadAsStringAsync());
    }
    
}
