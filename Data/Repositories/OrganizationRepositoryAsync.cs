using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Data.Contexts;
using Application.Interfaces.Repositories;
using Api.Application.Interfaces.Repositories;

namespace Api.Data.Persistence.Repository
{
    public class OrganizationRepositoryAsync : GenericRepositoryAsync<Organization>, IOrganizationRepositoryAsync
    {
        private readonly DbSet<Organization> _organizations;
        public OrganizationRepositoryAsync(ApiAdminDbContext dbContext) : base(dbContext)
        {
            _organizations = dbContext.Set<Organization>();
        }

        public async Task<Organization> OrganizationBySlugTenant(string slugTenant)
        {
            return await _organizations.Include(x=> x.UserOrganizations).ThenInclude(x=> x.User).FirstOrDefaultAsync(x => x.SlugTenant == slugTenant);
        }
    }
}
