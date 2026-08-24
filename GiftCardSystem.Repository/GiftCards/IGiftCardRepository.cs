using GiftCardSystem.Database.AppDbContextModels;

namespace GiftCardSystem.Repository.GiftCards
{
    public interface IGiftCardRepository
    {
        Task<bool> CreateGiftCard(GiftCard giftCard);
        Task<GiftCard?> GetByIdAsync(int id);
        Task<List<GiftCard>> GetActiveListAsync();
        Task<GiftCard?> GetByIdWithPaymentMethodsAsync(int id);
        Task ReplacePaymentMethodsAsync(int giftCardId, List<GiftCardPaymentMethod> newPaymentMethods);
        Task<bool> SaveChangesAsync();
    }
}
