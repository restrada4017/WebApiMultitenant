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
using Application.DTOs.Role;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Filters;
using Api.Application.Interfaces.Repositories;
using Api.Application.Interfaces;
using Api.Application.DTOs.Organization;
using Api.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{

    public class OrganizationService : IOrganizationService
    {
        private readonly IMapper _mapper;

        private readonly IOrganizationRepositoryAsync _organizationRepository;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IConfiguration _config;


        public OrganizationService(IMapper mapper, IOrganizationRepositoryAsync organizationRepository, IProductRepositoryAsync productRepository, IConfiguration config)
        {
            _mapper = mapper;
            _organizationRepository = organizationRepository;
            _productRepository = productRepository;
            _config = config;
        }

        public async Task<PagedResponse<IEnumerable<OrganizationListResponse>>> GetAll(RequestParameter requestParameter)
        {
            var roles = await _organizationRepository.GetPagedReponseAsync(requestParameter.PageNumber, requestParameter.PageSize);

            List<OrganizationListResponse> list = _mapper.Map<List<OrganizationListResponse>>(roles.ToList());

            return new PagedResponse<IEnumerable<OrganizationListResponse>>(list, requestParameter.PageNumber, requestParameter.PageSize);

        }

        public async Task<Response<OrganizationResponse>> OrganizationBySlugTenant(string slugTenant)
        {
            var validateSlugTenant = await _organizationRepository.OrganizationBySlugTenant(slugTenant);

            if (validateSlugTenant == null)
            {
                throw new ApiException($"SlugTenant does not exist");
            }

            OrganizationResponse organizationResponse = _mapper.Map<OrganizationResponse>(validateSlugTenant);

            var usersResponses = validateSlugTenant.UserOrganizations.Select(item => new UserResponse
            {
                Id = item.UserId,
                Email = item.User.Email,
            }).ToList();

            organizationResponse.Users = usersResponses;

            return new Response<OrganizationResponse>(organizationResponse);

        }

        public async Task<Response<string>> RegisterAsync(OrganizationRequest request)
        {
            var organization = _mapper.Map<Organization>(request);

            organization.SlugTenant = Guid.NewGuid().ToString();

            await _organizationRepository.AddAsync(organization);


            string connectionString = string.Format(_config.GetConnectionString("DefaultProduct"), "Product_" + organization.SlugTenant);

            await _productRepository.ExecuteMigration(connectionString);

            if (organization.Id != 0)
            {
                return new Response<string>(organization.Id.ToString(), message: $"Organization Registered.");
            }
            else
            {
                throw new ApiException($"User '{request.Name}' Not Registered");
            }
        }
    }
}


