namespace GiftCardSystem.Service.Store.Dtos;

public class GiftCardDetailDto
{
    public int GiftCardId { get; set; }
    public string GiftCardNo { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public List<PaymentOptionDto> PaymentOptions { get; set; } = new();
}

public class PaymentOptionDto
{
    public int PaymentMethodId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Discount { get; set; }
}
