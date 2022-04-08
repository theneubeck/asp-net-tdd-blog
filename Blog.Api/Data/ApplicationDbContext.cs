using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Data;

public class ApplicationDbContext : DbContext
{
    public string DbPath { get; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
            
    public ApplicationDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "tddblog.db");
    }
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}