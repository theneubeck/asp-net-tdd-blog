using System.Net;
using System.Net.Mime;
using Blog.Api.Data;
using Blog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("/")]
    public async Task<ActionResult> ListBlogPosts()
    {
        var list =  await _dbContext.Posts.ToListAsync();   
        return Ok(new BlogPostListResponse
        {
            Posts = list
        });
    }
    
    [HttpGet("/posts/{id}")]
    public async Task<ActionResult> GetBlogPost(Guid id)
    {
        var result =  await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpPut("/posts/{id}")]
    public async Task<ActionResult> EditBlogPost([FromBody] BlogPost post, Guid id)
    {
        var result =  await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
        {
            return NotFound();
        }
        result.Title = post.Title;
        result.Body = post.Body;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPost("/posts")]
    public async Task<ActionResult> CreateBlogPost([FromBody] BlogPost blogPostRequest)
    {
        blogPostRequest.Id = Guid.NewGuid();
        await _dbContext.Posts.AddAsync(blogPostRequest);
        await _dbContext.SaveChangesAsync();
        return new ObjectResult(blogPostRequest){StatusCode = (int) HttpStatusCode.Created};
    }
    
    [HttpDelete("/posts/{id}")]
    public async Task<ActionResult> DeleteBlogPost(Guid id)
    {
        var result =  await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
        {
            return NotFound();
        }

        _dbContext.Posts.Remove(result);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
    
}
