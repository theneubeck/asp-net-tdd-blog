using System.Net;
using System.Net.Mime;
using Blog.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private BlogDbContext _dbContext;

    public BlogController(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    [HttpPost("/posts")]
    public Task<ActionResult> CreateBlogPost()
    {
        return Task.FromResult<ActionResult>(new ObjectResult(new {Title = "Title", Body = "Body"}){StatusCode = (int) HttpStatusCode.Created});
    }
}
