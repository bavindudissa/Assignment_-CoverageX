using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using to_do_api;
using to_do_api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace to_do_api.Tests
{

    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TodoAppContext>));

                var conString = GetConnectionString();
                services.AddSqlServer<TodoAppContext>(conString);
            });
        }

        private static string? GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<CustomWebApplicationFactory>()
            .Build();

            var con = configuration.GetConnectionString("TestDb");
            return con;
        }
    }
}
