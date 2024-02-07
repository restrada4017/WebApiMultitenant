using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Data.Contexts;
using Application.Interfaces.Repositories;

namespace Api.Data.Persistence.Repository
{
    public class UserOrganizationRepositoryAsync : GenericRepositoryAsync<UserOrganization>, IUserOrganizationRepositoryAsync
    {
        private readonly DbSet<UserOrganization> _userOrganizations;
        public UserOrganizationRepositoryAsync(ApiAdminDbContext dbContext) : base(dbContext)
        {
            _userOrganizations = dbContext.Set<UserOrganization>();
        }

        public async Task<UserOrganization> ValideUserOrganization(int UserId, int OrganizationId)
        {
            return await _userOrganizations.Include(x=> x.Organization).FirstOrDefaultAsync(x => x.UserId == UserId && x.OrganizationId == OrganizationId);
        }
    }
}

