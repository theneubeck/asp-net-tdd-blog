using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests;

public class BlogPostCreationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private HttpClient _client;

    public BlogPostCreationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task CreateBlogPostTest()
    {
        var postBody = Utils.ToJsonStringContent(new {Title = "Title", Body = "Body"});
        var createResponse = await _client.PostAsync("/posts", postBody);

        var parsed = await createResponse.Content.ReadFromJsonAsync<BlogPostReponse>();
        var responseBody = await createResponse.Content.ReadAsStringAsync();
        var expected =
            Utils.ToJson(new BlogPostReponse() {Id = parsed.Id, Type = "post", Title = "Title", Body = "Body"});
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.Equal(expected, responseBody);

        var response = await _client.GetAsync($"/posts/{parsed.Id}");
        var post = await createResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, post);

        var listResponse = await _client.GetAsync($"/");
        var list = await listResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var expectedList = Utils.ToJson(new
        {
            Posts = new List<BlogPostReponse>()
            {
                new BlogPostReponse() {Id = parsed.Id, Type = "post", Title = "Title", Body = "Body"}
            }
        });
        Assert.Equal(expectedList, list);
    }
}
