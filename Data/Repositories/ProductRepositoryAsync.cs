using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiTenantApi.Data.Api;

namespace Api.Data.Persistence.Repository
{
    public class ProductRepositoryAsync : GenericProductRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;

        private readonly ProductDbContext _dbContext;

        public ProductRepositoryAsync(ProductDbContext dbContext) : base(dbContext)
        {
            try
            {
                _products = dbContext.Set<Product>();
                _dbContext = dbContext;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<bool> ExecuteMigration(string connectionString)
        {
            _dbContext.Database.SetConnectionString(connectionString);
            _dbContext.Database.Migrate();
            await Task.CompletedTask;
            return true;
        }
    }
}
