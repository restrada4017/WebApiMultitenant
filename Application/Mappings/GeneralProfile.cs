using Api.Application.DTOs.Organization;
using Api.Application.DTOs.User;
using Application.DTOs.Account;
using Application.DTOs.Role;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<OrganizationResponse, Organization>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<OrganizationResponse, Organization>().ReverseMap();
            CreateMap<OrganizationListResponse, Organization>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<OrganizationResponse, UserOrganization>();

            CreateMap<OrganizationResponse, Organization>()
            .ForMember(s => s.Id, c => c.MapFrom(m => m.Id))
            .ForMember(s => s.Name, c => c.MapFrom(m => m.Name))
            .ForMember(s => s.SlugTenant, c => c.MapFrom(m => m.SlugTenant)).ReverseMap();

            CreateMap<Organization, OrganizationRequest>().ReverseMap();




        }
    }
}
