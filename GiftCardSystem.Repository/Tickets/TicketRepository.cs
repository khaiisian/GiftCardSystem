using GiftCardSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace GiftCardSystem.Repository.Tickets;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _dbContext;

    public TicketRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HashSet<string>> GetAllPromoCodesAsync()
    {
        var codes = await _dbContext.Tickets.Select(t => t.PromoCode).ToListAsync();
        return codes.ToHashSet();
    }

    public async Task<int> AddTicketsAsync(List<Ticket> tickets)
    {
        await _dbContext.Tickets.AddRangeAsync(tickets);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Ticket>> GetAvailableTicketsAsync(int giftCardId, int count)
    {
        return await _dbContext.Tickets
            .Where(t => t.GiftCardId == giftCardId && t.Status == 0)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByPromoCodeAsync(string promoCode)
    {
        return await _dbContext.Tickets
            .Include(t => t.GiftCard)
            .FirstOrDefaultAsync(t => t.PromoCode == promoCode);
    }

    public async Task<List<Ticket>> GetOwnedTicketsAsync(int userId, byte status)
    {
        return await _dbContext.Tickets
            .Include(t => t.GiftCard)
            .Where(t => t.Status == status &&
                        t.TransactionDetails.Any(td => td.Transaction.UserId == userId))
            .ToListAsync();
    }
}
