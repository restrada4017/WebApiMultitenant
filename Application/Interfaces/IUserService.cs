using Api.Application.DTOs.User;
using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Response<AuthenticationResponse>> Login(AuthenticationRequest request);

        Task<Response<UserResponse>> GetUserByEmail(string email);

        Task<Response<string>> RegisterAsync(RegisterRequest request);

        Task<Response<bool>> AddOrganizationUser(OrganizationUserRequest request);

        Task<Response<bool>> RemoveOrganizationUser(OrganizationUserRequest request);
    }
}
