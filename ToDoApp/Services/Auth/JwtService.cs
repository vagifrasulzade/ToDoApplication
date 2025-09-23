﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Auth;

namespace ToDoApp.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtConfig _jwtConfig;

    public JwtService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string GenerateSecurityToken(
        string id, 
        string email, 
        IEnumerable<string> roles, 
        IEnumerable<Claim> userClaims)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(",", roles)),
            new Claim("userId", id)
        }.Concat(userClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.Expiration),
            signingCredentials: signingCredentials,
            claims: claims);

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}
