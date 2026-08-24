using GiftCardSystem.Database.AppDbContextModels;
using GiftCardSystem.Repository.GiftCards;
using GiftCardSystem.Service.GiftCards.Dtos;

namespace GiftCardSystem.Service.GiftCards;

public class GiftCardService
{
    private readonly IGiftCardRepository _repo;

    public GiftCardService(IGiftCardRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> CreateGiftCardAsync(CreateGiftCardDtos dto)
    {

        var giftCard = new GiftCard
        {
            GiftCardNo = $"GC-{Guid.NewGuid():N}",
            Title = dto.Title,
            Description = dto.Description,
            ExpiryDate = dto.ExpiryDate,
            Amount = dto.Amount,
            Quantity = dto.Quantity,
            Status = 1,                 // Active
            MaxPurchaseLimit = dto.MaxPurchaseLimit,
            GiftPerUserLimit = dto.GiftPerUserLimit,
            CreatedAt = DateTime.UtcNow,
            GiftCardPaymentMethods = (dto.GiftCardPaymentMethods).Select(x => new GiftCardPaymentMethod
            {
                PaymentMethodId = x.PaymentMethodId,
                Discount = x.Discount,
                CreatedAt = DateTime.UtcNow

            }).ToList()
        };

        var res = await _repo.CreateGiftCard(giftCard);
        return res;
    }

    public async Task<GiftCardResult> EditGiftCardAsync(int id, EditGiftCardDto dto)
    {
        var giftCard = await _repo.GetByIdAsync(id);
        if (giftCard is null) return GiftCardResult.NotFound;

        giftCard.Title = dto.Title;
        giftCard.Description = dto.Description;
        giftCard.ExpiryDate = dto.ExpiryDate;
        giftCard.Amount = dto.Amount;
        giftCard.Quantity = dto.Quantity;
        giftCard.MaxPurchaseLimit = dto.MaxPurchaseLimit;
        giftCard.GiftPerUserLimit = dto.GiftPerUserLimit;
        giftCard.ModifiedAt = DateTime.UtcNow;

        var newPaymentMethods = dto.GiftCardPaymentMethods.Select(x => new GiftCardPaymentMethod
        {
            PaymentMethodId = x.PaymentMethodId,
            Discount = x.Discount,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        await _repo.ReplacePaymentMethodsAsync(giftCard.GiftCardId, newPaymentMethods);

        var res = await _repo.SaveChangesAsync();
        return res ? GiftCardResult.Success : GiftCardResult.Failed;
    }

    public async Task<GiftCardResult> DeactivateGiftCardAsync(int id)
    {
        var giftCard = await _repo.GetByIdAsync(id);
        if (giftCard is null) return GiftCardResult.NotFound;

        giftCard.Status = 0;   // Deactivated
        giftCard.ModifiedAt = DateTime.UtcNow;

        var res = await _repo.SaveChangesAsync();
        return res ? GiftCardResult.Success : GiftCardResult.Failed;
    }
}

public enum GiftCardResult
{
    Success,
    NotFound,
    Failed
}
