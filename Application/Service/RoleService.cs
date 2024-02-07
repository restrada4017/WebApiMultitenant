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

namespace Application.Services
{

    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;

        private readonly IRoleRepositoryAsync _roleRepository;


        public RoleService(IMapper mapper, IRoleRepositoryAsync roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<PagedResponse<IEnumerable<RoleResponse>>> GetAll(RequestParameter requestParameter)
        {
            var roles = await _roleRepository.GetPagedReponseAsync(requestParameter.PageNumber, requestParameter.PageSize);

            List<RoleResponse> list = _mapper.Map<List<RoleResponse>>(roles.ToList());

            return new PagedResponse<IEnumerable<RoleResponse>>(list, requestParameter.PageNumber, requestParameter.PageSize);

        }
    }
}
