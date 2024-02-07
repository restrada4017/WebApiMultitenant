using Application.DTOs.Account;
using Application.DTOs.Role;
using Application.Filters;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResponse<IEnumerable<RoleResponse>>> GetAll(RequestParameter requestParameter);
    }
}

