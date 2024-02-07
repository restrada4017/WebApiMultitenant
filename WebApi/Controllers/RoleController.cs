using Application.DTOs.Account;
using Application.Exceptions;
using Application.Filters;
using Application.Interfaces;
using Application.Model.QueryFilters;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet, Route("GetAllRoles", Name = nameof(GetAllRoles))]
        public async Task<IActionResult> GetAllRoles([FromQuery] RequestParameter filters)
        {
            try
            {
                return Ok(await _roleService.GetAll(filters));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }

        }
    }
}

