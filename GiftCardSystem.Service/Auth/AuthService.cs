using GiftCardSystem.Database.AppDbContextModels;
using GiftCardSystem.Repository.Users;
using GiftCardSystem.Service.Auth.Dtos;
using Microsoft.Extensions.Configuration;

namespace GiftCardSystem.Service.Auth;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly TokenService _tokens;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository repo, TokenService tokens, IConfiguration config)
    {
        _repo = repo;
        _tokens = tokens;
        _config = config;
    }

    //Register
    public async Task<RegisterResult> Register(RegisterDto registerDto)
    {
        var user = await _repo.GetByEmailAsync(registerDto.Email);
        if (user is not null) return RegisterResult.EmailAlreadyExists;

        var newUser = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            PhNumber = registerDto.PhNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        var result = await _repo.AddUserAsync(newUser);

        if (!result) return RegisterResult.Failed;

        return RegisterResult.Success;
    }

    // Login
    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        var user = await _repo.GetByEmailAsync(loginDto.Email);
        if(user is null || !(verifyPassword(loginDto.Password, user.PasswordHash)))
        {
            return null;
        }

        var response = await issueTokenAsync(user);

        return response;
    }

    // Refresh Token
    public async Task<AuthResponseDto?> RefreshToken(RefreshDto refreshDto)
    {
        var user = await _repo.GetByRefreshTokenAsync(refreshDto.RefreshToken);
        if(user is null || (user.RefreshTokenExpiry < DateTime.UtcNow))
        {
            return null;
        }

        var response = await issueTokenAsync(user);

        return response;
    }

    private async Task<AuthResponseDto?> issueTokenAsync(User user)
    {
        var access = _tokens.CreateAccessToken(user);
        var refresh = _tokens.CreateRefreshToken();
        var refreshExpires = DateTime.UtcNow.AddDays(int.Parse(_config["JwtSetting:RefreshTokenDays"]!));

        bool result = await _repo.UpdateRefreshToken(user, refresh, refreshExpires);
        if (!result) return null;

        var minutes = int.Parse(_config["JwtSetting:AccessTokenMinutes"]!);

        var responeDto = new AuthResponseDto
        {
            AccessToken = access,
            RefreshToken = refresh,
            ExpiresIn = minutes * 60,
        };

        return responeDto;
    }

    private bool verifyPassword(string Password, string HashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(Password, HashPassword);
    }

}

public enum RegisterResult
{
    Success,
    EmailAlreadyExists,
    Failed
}
