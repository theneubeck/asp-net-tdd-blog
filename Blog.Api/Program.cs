using Blog.Api.Data;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
var app = builder.Build();
app.UseHttpLogging();
InitializeDatabase(app);
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

