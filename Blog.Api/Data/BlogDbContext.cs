using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Data;

public sealed class BlogDbContext : DbContext
{

    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options) { }

    public DbSet<BlogPost> Posts { get; set; }

}
