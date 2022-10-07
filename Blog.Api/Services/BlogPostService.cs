using Blog.Api.Data;
using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services;

public class BlogPostService : IBlogPostService
{
    private readonly BlogDbContext _dbContext;

    public BlogPostService(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BlogPost>> GetBlogPostsAsync()
    {
        return await _dbContext.Posts.ToListAsync();
    }

    public async Task<bool> CreateBlogPostAsync(BlogPost newPost)
    {
        newPost.Id = Guid.NewGuid();
        _dbContext.Posts.Add(newPost);

        var saveResult = await _dbContext.SaveChangesAsync();
        return saveResult == 1;
    }

    public async Task<BlogPost?> GetById(Guid id)
    {
        return await _dbContext.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateBlogPostAsync(Guid id, BlogPost updatePost)
    {
        var post = await GetById(id);
        if (post == null) return false;
        post.Body = updatePost.Body;
        post.Title = updatePost.Title;

        var saveResult = await _dbContext.SaveChangesAsync();
        return saveResult == 1;
    }
}
