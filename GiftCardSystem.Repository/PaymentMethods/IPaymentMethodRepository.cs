using GiftCardSystem.Database.AppDbContextModels;

namespace GiftCardSystem.Repository.PaymentMethods;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethod>> GetAllAsync();
}
