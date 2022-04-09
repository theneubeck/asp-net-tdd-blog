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

  [HttpGet("/posts/{id}")]
  public IActionResult Get([FromRoute] Guid id)
  {
      if (id == Guid.Empty)
      {
          return BadRequest(new
          {
              Errors = new[] {new {Status = 400, Title = "BadRequest", Message = "Invalid supplierId"}}
          });
      }

      return Ok(new BlogPost()
      {
          Id = id,
          Title = "Title",
          Body = "Body"
      });
  }

  [HttpPost("/posts")]
  public IActionResult Create()
  {
    return new ObjectResult(new BlogPost()
    {
        Id = Guid.NewGuid(),
        Title = "Title",
        Body = "Body"
    }) { StatusCode = StatusCodes.Status201Created};
  }

}
