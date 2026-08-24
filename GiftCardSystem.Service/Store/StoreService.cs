using GiftCardSystem.Repository.GiftCards;
using GiftCardSystem.Service.Store.Dtos;

namespace GiftCardSystem.Service.Store;

public class StoreService
{
    private readonly IGiftCardRepository _repo;

    public StoreService(IGiftCardRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<GiftCardListDto>> GetListAsync()
    {
        var cards = await _repo.GetActiveListAsync();
        return cards.Select(c => new GiftCardListDto
        {
            GiftCardId = c.GiftCardId,
            Title = c.Title,
            Amount = c.Amount,
            ExpiryDate = c.ExpiryDate
        }).ToList();
    }

    public async Task<GiftCardDetailDto?> GetDetailAsync(int id)
    {
        var card = await _repo.GetByIdWithPaymentMethodsAsync(id);
        if (card is null || card.Status != 1) return null;

        return new GiftCardDetailDto
        {
            GiftCardId = card.GiftCardId,
            GiftCardNo = card.GiftCardNo,
            Title = card.Title,
            Description = card.Description,
            Amount = card.Amount,
            ExpiryDate = card.ExpiryDate,
            PaymentOptions = card.GiftCardPaymentMethods.Select(pm => new PaymentOptionDto
            {
                PaymentMethodId = pm.PaymentMethodId,
                Name = pm.PaymentMethod.Name,
                Discount = pm.Discount
            }).ToList()
        };
    }
}
