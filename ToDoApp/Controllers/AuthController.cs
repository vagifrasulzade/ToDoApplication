using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;

using ToDoApp.DTOs.Auth;
using ToDoApp.Models;
using ToDoApp.Services.Auth;


namespace ToDoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signManager;
        _jwtService = jwtService;
    }

    /// <summary>
    ///  Login
    /// </summary>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ActionResult<AuthTokenDTO>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return Unauthorized();

        var canSignIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!canSignIn.Succeeded) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new[]
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email!),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(",", roles)),
            new Claim("userId", user.Id)
            
        }.Concat(userClaims);


        var accessToken = _jwtService.GenerateSecurityToken(user.Id, user.Email!, roles, userClaims);

        var refreshToken = Guid.NewGuid().ToString("N").ToLower();

        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        return new AuthTokenDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    

    /// <summary>
    /// Register
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    [HttpPost("Register")]
    public async Task<ActionResult<AuthTokenDTO>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null) return Conflict("User already exists.");

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BadRequest("Bad request");

        await _userManager.UpdateAsync(user);

        return await GenerateToken(user);
    }



    /// <summary>
    /// Refresh 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Refresh")]
    public async Task<ActionResult<AuthTokenDTO>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user is null) return Unauthorized();

        return await GenerateToken(user);
    }

    private async Task<AuthTokenDTO> GenerateToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);


        var accessToken = _jwtService.GenerateSecurityToken(user.Id, user.Email!, roles, userClaims);

        var refreshToken = Guid.NewGuid().ToString("N").ToLower();
        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        return new AuthTokenDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }


}