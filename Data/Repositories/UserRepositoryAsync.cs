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
    public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
    {
        private readonly DbSet<User> _users;

        public UserRepositoryAsync(ApiAdminDbContext dbContext) : base(dbContext)
        {
            _users = dbContext.Set<User>();
        }

        public async Task<User> Login(string email, string password)
        {
            var obj = await _users.Include(x => x.Role).Include(x => x.UserOrganizations).ThenInclude(x => x.Organization).FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            return obj;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _users.Include(x => x.Role).Include(x => x.UserOrganizations).ThenInclude(x => x.Organization).FirstOrDefaultAsync(x => x.Email == email);
        }

    }
}
