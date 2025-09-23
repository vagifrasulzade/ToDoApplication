﻿using System.Security.Claims;

namespace ToDoApp.Services.Auth;

public interface IJwtService
{
    string GenerateSecurityToken(
        string id,
        string email,
        IEnumerable<string> roles,
        IEnumerable<Claim> userClaims
        );
}
