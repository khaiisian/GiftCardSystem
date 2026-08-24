using GiftCardSystem.Api.Helper;
using GiftCardSystem.Service.Auth;
using GiftCardSystem.Service.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly HttpContextService _httpContextService;

    public AuthController(AuthService authService, HttpContextService httpContextService)
    {
        _authService = authService;
        _httpContextService = httpContextService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.Register(registerDto);
        return result switch
        {
            RegisterResult.Success            => Ok("Registered successfully."),
            RegisterResult.EmailAlreadyExists => Conflict("Email already used."),
            RegisterResult.Failed             => StatusCode(500, "Registration failed."),
            _                                 => StatusCode(500, "Unknown error.")
        };
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        return result is null ? Unauthorized("Invalid credentials.") : Ok(result);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshDto refreshDto)
    {
        var result = await _authService.RefreshToken(refreshDto);
        return result is null ? Unauthorized("Invalid or expired refresh token.") : Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        return Ok(_httpContextService.UserEmail());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("You are an admin");
    }

    //just to get hashvalue for my own testing
    [HttpPost("Hashpassword")]
    public IActionResult HashPassword([FromBody]string password)
    {
        return Ok(BCrypt.Net.BCrypt.HashPassword(password));
    }
}
