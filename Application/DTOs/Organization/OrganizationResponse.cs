using Api.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.DTOs.Organization
{
    public class OrganizationResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string SlugTenant { get; set; }

        public List<UserResponse> Users { get; set; } = new List<UserResponse>();
    }
}
