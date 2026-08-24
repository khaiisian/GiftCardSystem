namespace GiftCardSystem.Service.GiftCards.Dtos;

public class EditGiftCardDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal Amount { get; set; }
    public int Quantity { get; set; }
    public int? MaxPurchaseLimit { get; set; }
    public int? GiftPerUserLimit { get; set; }

    public List<GiftCardPaymentMethodDto> GiftCardPaymentMethods { get; set; } = new();
}
