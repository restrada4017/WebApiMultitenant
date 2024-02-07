﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
