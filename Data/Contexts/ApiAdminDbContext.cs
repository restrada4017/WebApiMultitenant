using Api.Data.Seeds;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Contexts;

public class ApiAdminDbContext : DbContext
{

    public ApiAdminDbContext(DbContextOptions<ApiAdminDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserOrganization> UserOrganizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleSeed());
        modelBuilder.ApplyConfiguration(new UserSeed());
    }
}

