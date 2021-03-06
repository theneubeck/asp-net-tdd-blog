using System.Net.Mime;
using Blog.Api.Models;
using Blog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly ILogger<BlogController> _logger;
    private readonly IBlogPostService _blogPostService;

    public BlogController(ILogger<BlogController> logger, IBlogPostService blogPostService)
    {
        _logger = logger;
        _blogPostService = blogPostService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        var posts = await _blogPostService.GetBlogPostsAsync();
        return Ok(new BlogPostList() {Posts = posts});
    }

    [HttpGet("/posts/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new
            {
                Errors = new[] {new {Status = 400, Title = "BadRequest", Message = "Invalid supplierId"}}
            });
        }
        
        var post = await _blogPostService.GetById(id);
        if (post == null)
        {
            return NotFound();
        }
        
        return Ok(post);
    }

    [HttpPost("/posts")]
    public async Task<IActionResult> Create([FromBody] BlogPost newPost)
    {
        var success = await _blogPostService.CreateBlogPostAsync(newPost);
        if (success)
        {
            return BadRequest(new
            {
                Errors = new[] {new {Status = 400, Title = "BadRequest", Message = "Invalid post"}}
            });
        }

        return new ObjectResult(new BlogPost() {Id = Guid.NewGuid(), Title = "Title", Body = "Body"})
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}

