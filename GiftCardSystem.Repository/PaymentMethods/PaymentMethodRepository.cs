using GiftCardSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace GiftCardSystem.Repository.PaymentMethods;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _dbContext;

    public PaymentMethodRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<PaymentMethod>> GetAllAsync()
    {
        return await _dbContext.PaymentMethods.ToListAsync();
    }
}
