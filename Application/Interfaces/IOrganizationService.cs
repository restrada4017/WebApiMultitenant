using Api.Application.DTOs.Organization;
using Application.DTOs.Role;
using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Interfaces
{
    public interface IOrganizationService
    {
        Task<PagedResponse<IEnumerable<OrganizationListResponse>>> GetAll(RequestParameter requestParameter);

        Task<Response<string>> RegisterAsync(OrganizationRequest request);

        Task<Response<OrganizationResponse>> OrganizationBySlugTenant(string slugTenant);
    }
}


