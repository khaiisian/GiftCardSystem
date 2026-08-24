using GiftCardSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace GiftCardSystem.Repository.Users;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        return user;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        int result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> UpdateRefreshToken(User user, string refresh, DateTime refreshExpires)
    {
        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = refreshExpires;
        int result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
}
