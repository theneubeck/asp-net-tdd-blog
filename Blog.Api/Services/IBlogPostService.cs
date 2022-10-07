using Blog.Api.Models;

namespace Blog.Api.Services;
public interface IBlogPostService
{
  Task<List<BlogPost>> GetBlogPostsAsync();
  Task<bool> CreateBlogPostAsync(BlogPost newPost);
  Task<BlogPost?> GetById(Guid id);
  Task<bool> UpdateBlogPostAsync(Guid id, BlogPost updatePost);
}
