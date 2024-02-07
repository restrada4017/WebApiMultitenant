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
    public class RoleRepositoryAsync : GenericRepositoryAsync<Role>, IRoleRepositoryAsync
    {
        public RoleRepositoryAsync(ApiAdminDbContext dbContext) : base(dbContext)
        {
        }
    }
}
