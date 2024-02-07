using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Interfaces.Repositories
{
    public interface IOrganizationRepositoryAsync : IGenericRepositoryAsync<Organization>
    {
       Task<Organization> OrganizationBySlugTenant(string slugTenant);
    }
}
