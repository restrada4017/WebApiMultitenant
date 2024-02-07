using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        public required int RoleId { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        public string? SlugTenant { get; set; }
    }
}
