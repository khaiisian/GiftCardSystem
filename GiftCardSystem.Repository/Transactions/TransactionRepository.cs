using GiftCardSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace GiftCardSystem.Repository.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> CountSelfPurchasedAsync(int userId, int giftCardId)
    {
        var result = _dbContext.TransactionDetails.CountAsync(td =>
            td.Transaction.UserId == userId &&
            td.Transaction.GiftCardId == giftCardId &&
            td.Transaction.TypeOfBuying == 0);
        return result;
    }

    public Task<int> CountGiftedByBuyerAsync(int userId, int giftCardId)
    {
        var result = _dbContext.TransactionDetails.CountAsync(td =>
            td.Transaction.UserId == userId &&
            td.Transaction.GiftCardId == giftCardId &&
            td.Transaction.TypeOfBuying == 1);
        return result;
    }

    public Task<int> CountGiftedToRecipientAsync(string recipientPhone, int giftCardId)
    {
        return _dbContext.TransactionDetails.CountAsync(td =>
            td.Transaction.RecipientPhone == recipientPhone &&
            td.Transaction.GiftCardId == giftCardId &&
            td.Transaction.TypeOfBuying == 1);
    }

    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        _dbContext.Transactions.Add(transaction);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<Transaction>> GetByUserAsync(int userId)
    {
        return await _dbContext.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.GiftCard)
            .Include(t => t.TransactionDetails)
                .ThenInclude(td => td.Ticket)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }
}
