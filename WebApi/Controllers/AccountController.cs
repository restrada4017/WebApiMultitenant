using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            try
            {
                return Ok(await _userService.Login(request));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }

        }

        [HttpPost, Route("CreateUser", Name = nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(RegisterRequest registerRequest)
        {
            try
            {
                return Ok(await _userService.RegisterAsync(registerRequest));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        [HttpPost, Route("AddOrganizationUser", Name = nameof(AddOrganizationUser))]
        public async Task<IActionResult> AddOrganizationUser(OrganizationUserRequest request)
        {
            try
            {
                return Ok(await _userService.AddOrganizationUser(request));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        [HttpPost, Route("RemoveOrganizationUser", Name = nameof(RemoveOrganizationUser))]
        public async Task<IActionResult> RemoveOrganizationUser(OrganizationUserRequest request)
        {
            try
            {
                return Ok(await _userService.RemoveOrganizationUser(request));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        [HttpGet, Route("GetUserByEmail", Name = nameof(GetUserByEmail))]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmail(email));
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }
    }
}
