using System;
using System.Linq;
using Blog.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blog.Tests.Setup
{
    [Collection("DbTestCollection")]
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Lazy<IServiceScope> _testServiceScope;

        public IServiceScope ServiceScope => _testServiceScope.Value;

        public BlogDbContext DbContext => _testServiceScope.Value.ServiceProvider.GetRequiredService<BlogDbContext>();


        public TestWebApplicationFactory()
        {
            _testServiceScope =
                new Lazy<IServiceScope>(() => Services.GetRequiredService<IServiceScopeFactory>().CreateScope());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor =
                        services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));
                    services.Remove(descriptor!);
                    services.AddDbContext<BlogDbContext>(options =>
                    {
                        options.UseSqlite(Config.DbTestConnectionString);
                    });
                });
        }
    }
}
