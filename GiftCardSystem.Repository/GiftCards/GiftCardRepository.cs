using GiftCardSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace GiftCardSystem.Repository.GiftCards;

public class GiftCardRepository : IGiftCardRepository
{
    private readonly AppDbContext _dbContext;

    public GiftCardRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateGiftCard(GiftCard giftCard)
    {
        await _dbContext.GiftCards.AddAsync(giftCard);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<GiftCard?> GetByIdAsync(int id)
    {
        var item = await _dbContext.GiftCards.FirstOrDefaultAsync(x => x.GiftCardId == id);
        return item;
    }

    public async Task<List<GiftCard>> GetActiveListAsync()
    {
        var lst = await _dbContext.GiftCards.Where(x => x.Status == 1).ToListAsync();
        return lst;
    }

    public async Task<GiftCard?> GetByIdWithPaymentMethodsAsync(int id)
    {
        var item = await _dbContext.GiftCards
            .Include(x => x.GiftCardPaymentMethods)
                .ThenInclude(pm => pm.PaymentMethod)
            .FirstOrDefaultAsync(x => x.GiftCardId == id);
        return item;
    }

    public async Task ReplacePaymentMethodsAsync(int giftCardId, List<GiftCardPaymentMethod> newPaymentMethods)
    {
        var existing = await _dbContext.GiftCardPaymentMethods
            .Where(x => x.GiftCardId == giftCardId)
            .ToListAsync();

        _dbContext.GiftCardPaymentMethods.RemoveRange(existing);

        foreach (var pm in newPaymentMethods)
            pm.GiftCardId = giftCardId;

        await _dbContext.GiftCardPaymentMethods.AddRangeAsync(newPaymentMethods);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
