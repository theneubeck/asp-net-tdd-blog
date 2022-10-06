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

public class BlogPostCreationTest : IntegrationTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;

    public BlogPostCreationTest(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CreateBlogPostTest()
    {
        var postBody = Utils.ToJsonStringContent(new {Title = "Title", Body = "Some valid body"});
        var createResponse = await _client.PostAsync("/posts", postBody);

        var parsed = await createResponse.Content.ReadFromJsonAsync<BlogPostReponse>();
        var responseBody = await createResponse.Content.ReadAsStringAsync();
        var expected =
            Utils.ToJson(new BlogPostReponse() {Id = parsed.Id, Type = "post", Title = "Title", Body = "Some valid body"});
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, responseBody);
        responseBody.Should().Be(expected);

        var response = await _client.GetAsync($"/posts/{parsed.Id}");
        var post = await createResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, post);

        var listResponse = await _client.GetAsync($"/");
        var actualList     = await listResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        var expectedList = Utils.ToJson(new
        {
            Posts = new List<BlogPostReponse>()
            {
                new() {Id = parsed.Id, Type = "post", Title = "Title", Body = "Some valid body"}
            }
        });
        Assert.Equal(expectedList, actualList);

    }
    
    [Fact]
    public async Task CreateBlogPostWithNoBodyNorTitleTest()
    {
        var postBody = Utils.ToJsonStringContent(new {});
        var createResponse = await _client.PostAsync("/posts", postBody);
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, await createResponse.Content.ReadAsStringAsync());
        
        var parsed = await createResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        parsed.Status.Should().Be(400);
        parsed.Errors.Body.Should().Equal(new List<string>{"The Body field is required."});
        parsed.Errors.Title.Should().Equal(new List<string>{"The Title field is required."});
        var listResponse = await _client.GetAsync($"/");
        var actualList     = await listResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        var expectedList = Utils.ToJson(new { Posts = new List<BlogPostReponse>() });
        Assert.Equal(expectedList, actualList);
    }
    [Fact]
    public async Task CreateBlogPostWithTooShortBodyTest()
    {
        var postBody = Utils.ToJsonStringContent(new {Title = "Title", Body = "Body"});
        var createResponse = await _client.PostAsync("/posts", postBody);
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, await createResponse.Content.ReadAsStringAsync());
        
        var parsed = await createResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        parsed.Status.Should().Be(400);
        parsed.Errors.Body.First().Should().Be("Body must be at least 10 chars");
        var listResponse = await _client.GetAsync($"/");
        var actualList     = await listResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        var expectedList = Utils.ToJson(new { Posts = new List<BlogPostReponse>() });
        Assert.Equal(expectedList, actualList);
    }
}

public class ErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public ErrorModel Errors { get; set; }
    public int Status { get; set; }

    public class ErrorModel
    {
        public List<string> Body { get; set; }
        public List<string> Title { get; set; }
    }
}
