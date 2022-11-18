using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Data;

public sealed class BlogDbContext : DbContext
{

    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options) { }
    
}
