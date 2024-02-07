using Api.Application.DTOs.Organization;
using Api.Application.Interfaces;
using Application.DTOs.Account;
using Application.Exceptions;
using Application.Filters;
using Application.Interfaces;
using Application.Model.QueryFilters;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Authorize(Roles = "Administrators")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet, Route("GetAllOrganization", Name = nameof(GetAllOrganization))]
        public async Task<IActionResult> GetAllOrganization([FromQuery] RequestParameter filters)
        {
            try
            {
                return Ok(await _organizationService.GetAll(filters));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }

        }

        [HttpPost, Route("CreateOrganization", Name = nameof(CreateOrganization))]
        public async Task<IActionResult> CreateOrganization(OrganizationRequest registerRequest)
        {
            try
            {
                return Ok(await _organizationService.RegisterAsync(registerRequest));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        [Authorize(Roles = "Administrators,Organizations")]
        [HttpGet, Route("OrganizationBySlugTenant", Name = nameof(OrganizationBySlugTenant))]
        public async Task<IActionResult> OrganizationBySlugTenant(string slugTenant)
        {
            try
            {
                return Ok(await _organizationService.OrganizationBySlugTenant(slugTenant));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }
    }
}


