using GiftCardSystem.Database.AppDbContextModels;

namespace GiftCardSystem.Repository.Users
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> UpdateRefreshToken(User user, string refresh, DateTime refreshExpires);
    }
}