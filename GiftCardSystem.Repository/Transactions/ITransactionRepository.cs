using GiftCardSystem.Database.AppDbContextModels;

namespace GiftCardSystem.Repository.Transactions;

public interface ITransactionRepository
{
    Task<int> CountSelfPurchasedAsync(int userId, int giftCardId);
    Task<int> CountGiftedByBuyerAsync(int userId, int giftCardId);
    Task<int> CountGiftedToRecipientAsync(string recipientPhone, int giftCardId);
    Task<bool> AddTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetByUserAsync(int userId);
}
