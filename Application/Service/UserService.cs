using AutoMapper;
using MediatR;

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Application.Model.QueryFilters;
using Application.Interfaces.Repositories;
using Application.DTOs.Account;
using Application.Exceptions;
using Domain.Settings;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Application.Wrappers;
using System.Net;
using System.Security.Cryptography;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Api.Application.Enum;
using Api.Application.Interfaces.Repositories;
using Application.DTOs.User;
using Application.DTOs.Role;
using Api.Application.DTOs.User;
using Application.Filters;
using Api.Application.DTOs.Organization;

namespace Application.Services
{

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly JWTSettings _jwtSettings;
        private readonly IRoleRepositoryAsync _roleRepository;
        private readonly IOrganizationRepositoryAsync _organizationRepository;
        private readonly IUserOrganizationRepositoryAsync _userOrganizationRepository;




        public UserService(IMapper mapper,
            IUserRepositoryAsync userRepository,
            IOptions<JWTSettings> jwtSettings,
            IRoleRepositoryAsync roleRepository,
            IOrganizationRepositoryAsync organizationRepository,
            IUserOrganizationRepositoryAsync userOrganizationRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<Response<UserResponse>> GetUserByEmail(string email)
        {
            var userValidate = await _userRepository.GetUserByEmail(email);
            if (userValidate == null)
            {
                throw new ApiException($"No Accounts Registered with {email}.");
            }

            UserResponse userResponse = _mapper.Map<UserResponse>(userValidate);

            var organizationResponses = userValidate.UserOrganizations.Select(item => new OrganizationResponse
            {
                Id = item.OrganizationId,
                Name = item.Organization.Name,
                SlugTenant = item.Organization.SlugTenant,
            }).ToList();

            userResponse.Organizations = organizationResponses;

            return new Response<UserResponse>(userResponse);

        }

        public async Task<Response<AuthenticationResponse>> Login(AuthenticationRequest request)
        {

            var userValidate = await _userRepository.GetUserByEmail(request.Email);
            if (userValidate == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }

            // Hash the password
            SHA256Managed sha = new SHA256Managed();
            byte[] buffer = Encoding.Default.GetBytes(request.Password == null ? string.Empty : request.Password);
            byte[] dataCifrada = sha.ComputeHash(buffer);
            string strPassword = BitConverter.ToString(dataCifrada).Replace("-", "");

            var user = await _userRepository.Login(request.Email, strPassword);
            if (user == null)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id.ToString();
            response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            response.Tenants = user.UserOrganizations.Select(item => new TenantsResponse
            {
                SlugTenant = item.Organization.SlugTenant,
            }).ToList();
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.Email}");

        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
            claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            await Task.CompletedTask;

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(1),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameUserName = await _userRepository.GetUserByEmail(request.Email);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Email '{request.Email}' is already taken.");
            }

            if (request.RoleId == 0)
            {
                throw new ApiException($"RoleId Required!");
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new ApiException($"Role '{request.RoleId}' not exist");
            }

            if ((int)EnumRole.Administrators == request.RoleId && !string.IsNullOrEmpty(request.SlugTenant))
            {
                throw new ApiException($"Can't associate SlugTenant with an admin user");
            }

            var user = _mapper.Map<User>(request);

            // Hash the password
            SHA256Managed sha = new SHA256Managed();
            byte[] buffer = Encoding.Default.GetBytes(user.Password == null ? string.Empty : user.Password);
            byte[] dataCifrada = sha.ComputeHash(buffer);
            string strPassword = BitConverter.ToString(dataCifrada).Replace("-", "");

            user.Password = strPassword;

            if (!string.IsNullOrEmpty(request.SlugTenant))
            {
                var validateSlugTenant = await _organizationRepository.OrganizationBySlugTenant(request.SlugTenant);

                if (validateSlugTenant == null)
                {
                    throw new ApiException($"SlugTenant does not exist");
                }

                user.UserOrganizations = new List<UserOrganization>
                {
                    new UserOrganization
                    {
                        OrganizationId = validateSlugTenant.Id
                    }
                };
            }

            await _userRepository.AddAsync(user);

            if (user.Id != 0)
            {
                return new Response<string>(user.Id.ToString(), message: $"User Registered.");
            }
            else
            {
                throw new ApiException($"User '{request.Email}' Not Registered");
            }
        }

        public async Task<Response<bool>> AddOrganizationUser(OrganizationUserRequest request)
        {
            var userWithSameUserName = await _userRepository.GetUserByEmail(request.Email);
            if (userWithSameUserName == null)
            {
                throw new ApiException($"Email '{request.Email}' does not exist");
            }

            var validateSlugTenant = await _organizationRepository.OrganizationBySlugTenant(request.SlugTenant);

            if (validateSlugTenant == null)
            {
                throw new ApiException($"SlugTenant does not exist");
            }

            var valideUserOrganization = await _userOrganizationRepository.ValideUserOrganization(userWithSameUserName.Id, validateSlugTenant.Id);

            if (valideUserOrganization != null)
            {
                throw new ApiException($"the user '{request.Email}' already has the organization '{valideUserOrganization.Organization.Name}' associated");
            }

            var userOrganization = new UserOrganization
            {
                UserId = userWithSameUserName.Id,
                OrganizationId = validateSlugTenant.Id
            };

            await _userOrganizationRepository.AddAsync(userOrganization);

            if (userOrganization.Id != 0)
            {
                return new Response<bool>(true, message: $"SlugTenant Registered.");
            }
            else
            {
                throw new ApiException($"SlugTenant '{request.SlugTenant}' Not Registered");
            }
        }

        public async Task<Response<bool>> RemoveOrganizationUser(OrganizationUserRequest request)
        {
            var userWithSameUserName = await _userRepository.GetUserByEmail(request.Email);
            if (userWithSameUserName == null)
            {
                throw new ApiException($"Email '{request.Email}' does not exist");
            }

            var validateSlugTenant = await _organizationRepository.OrganizationBySlugTenant(request.SlugTenant);

            if (validateSlugTenant == null)
            {
                throw new ApiException($"SlugTenant does not exist");
            }

            var valideUserOrganization = await _userOrganizationRepository.ValideUserOrganization(userWithSameUserName.Id, validateSlugTenant.Id);

            if (valideUserOrganization == null)
            {
                throw new ApiException($"the user '{request.Email}' not already has the organization '{validateSlugTenant.Name}' associated");
            }

            await _userOrganizationRepository.DeleteAsync(valideUserOrganization);

            return new Response<bool>(true, message: $"User removed from organization");

        }
    }
}




