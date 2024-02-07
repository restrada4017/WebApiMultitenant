using Api.Application.DTOs.Organization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public List<TenantsResponse> Tenants { get; set; } = new List<TenantsResponse>();
    }
}
