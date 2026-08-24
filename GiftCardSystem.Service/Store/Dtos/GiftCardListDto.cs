namespace GiftCardSystem.Service.Store.Dtos;

public class GiftCardListDto
{
    public int GiftCardId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
}
