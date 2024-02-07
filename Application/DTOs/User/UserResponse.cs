using Api.Application.DTOs.Organization;
using Application.DTOs.Role;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.DTOs.User
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public List<OrganizationResponse> Organizations { get; set; } = new List<OrganizationResponse>();

        public RoleResponse Role { get; set; }
    }
}
