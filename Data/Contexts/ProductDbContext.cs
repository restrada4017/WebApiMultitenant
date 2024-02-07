using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MultiTenantApi.Data.Api;

public class ProductDbContext : DbContext
{
    private readonly IConfiguration _config;
    private readonly HttpContext? _httpContext;
    private readonly IWebHostEnvironment _env;

    public ProductDbContext(
        DbContextOptions<ProductDbContext> options,
        IConfiguration config,
        IHttpContextAccessor contextAccessor,
        IWebHostEnvironment env
        )
        : base(options)
    {
        _config = config;
        _httpContext = contextAccessor.HttpContext;
        _env = env;
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {



        string connectionString = string.Empty;
        string tenantName = string.Empty;

        try
        {
            tenantName = GetTenantAndPathFrom(_httpContext.Request);
        }
        catch (Exception ex)
        {

        }

        if (string.IsNullOrEmpty(tenantName) && _env.IsDevelopment())
        {
            // Init/Dev connection string
            connectionString = _config.GetConnectionString("DefaultConnection");
        }
        else
        {

            connectionString = string.Format(_config.GetConnectionString("DefaultProduct"), "Product_" + tenantName);
        }


        optionsBuilder.UseSqlServer(connectionString);


    }

    private string GetTenantAndPathFrom(HttpRequest httpRequest)
    {
        // example: https://localhost/tenant1 -> gives tenant1
        var tenantName = new Uri(httpRequest.GetDisplayUrl())
            .Segments
            .FirstOrDefault(x => x != "/")
            ?.TrimEnd('/');

        if (!string.IsNullOrWhiteSpace(tenantName) &&
            httpRequest.Path.StartsWithSegments($"/{tenantName}",
                out PathString realPath))
        {
            return tenantName;
        }

        return (string.Empty);
    }
}