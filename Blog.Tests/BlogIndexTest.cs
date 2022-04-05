using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Blog.Tests;

public class BlogTest : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;
  private HttpClient _client;

  public BlogTest(WebApplicationFactory<Program> factory)
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
    // Assert.Equal(, body);
    // var expected = JsonSerializer.Serialize(new BlogListReponse { Posts = new List<BlogPostReponse>() }, new JsonSerializerOptions
    // {
    //   PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    // });
    var expected = ToJson(new { Posts = new List<BlogPostReponse>() });
    Assert.Equal(expected, responseBody);
  }


  [Fact]
  public async Task BlogIndexTest()
  {
    var body = JsonSerializer.Serialize<object>(new { Title = "Title", Body = "Body" }, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    var response = await _client.PostAsync("/posts", new StringContent(body));
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
  }

  public static string ToJson<T>(T obj)
  {
    return JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
  }
}

internal class BlogListReponse
{
  public List<BlogPostReponse> Posts { get; set; }
}

public class BlogPostReponse
{
  public string Title { get; set; }
  public System.Guid Id { get; set; }
  public string Body { get; set; }
}