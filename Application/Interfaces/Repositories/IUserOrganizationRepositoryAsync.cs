using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserOrganizationRepositoryAsync : IGenericRepositoryAsync<UserOrganization>
    {
        Task<UserOrganization> ValideUserOrganization(int UserId, int OrganizationId);    
    }
}
