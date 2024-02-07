using Api.Application.Interfaces.Repositories;
using Api.Data.Contexts;
using Api.Data.Persistence.Repository;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantApi.Data.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Data
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApiAdminDbContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("ApiAdminDb"),
               b => b.MigrationsAssembly(typeof(ApiAdminDbContext).Assembly.FullName)));

            services.AddDbContext<ProductDbContext>();
                      

            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericProductRepositoryAsync<>));
            services.AddTransient<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddTransient<IRoleRepositoryAsync, RoleRepositoryAsync>();
            services.AddTransient<IOrganizationRepositoryAsync, OrganizationRepositoryAsync>();
            services.AddTransient<IUserOrganizationRepositoryAsync, UserOrganizationRepositoryAsync>();
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            #endregion
        }
    }
}
