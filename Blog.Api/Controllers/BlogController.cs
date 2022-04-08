using Blog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
  private readonly ILogger<BlogController> _logger;

  public BlogController(ILogger<BlogController> logger)
  {
    _logger = logger;
  }

  [HttpGet("/")]
  public IActionResult Index()
  {
    return Ok(new BlogPostList() { });
  }

  [HttpPost("/posts")]
  public IActionResult Create()
  {
    return new ObjectResult(new BlogPostList() { }) { StatusCode = StatusCodes.Status201Created};
  }

}
