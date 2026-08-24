using GiftCardSystem.Database.AppDbContextModels;

namespace GiftCardSystem.Repository.Tickets;

public interface ITicketRepository
{
    Task<HashSet<string>> GetAllPromoCodesAsync();
    Task<int> AddTicketsAsync(List<Ticket> tickets);
    Task<List<Ticket>> GetAvailableTicketsAsync(int giftCardId, int count);
    Task<Ticket?> GetByPromoCodeAsync(string promoCode);
    Task<List<Ticket>> GetOwnedTicketsAsync(int userId, byte status);
}
