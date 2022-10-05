using Blog.Api.Data;
using Blog.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Console.WriteLine("**ENV is**");
// Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
// Console.WriteLine(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"));
// Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBlogPostService, BlogPostService>();

var app = builder.Build();
InitializeDatabase(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

void InitializeDatabase(IApplicationBuilder appBuilder)
{
    using IServiceScope service = appBuilder.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();


    var context = service.ServiceProvider
        .GetRequiredService<BlogDbContext>();
    // To disable migrations when using in memory db
    if (context.Database.IsRelational())
    {
        context.Database.Migrate();
    }
}

app.Run();

public partial class Program { }

