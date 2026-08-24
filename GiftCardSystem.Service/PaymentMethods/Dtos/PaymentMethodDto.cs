namespace GiftCardSystem.Service.PaymentMethods.Dtos;

public class PaymentMethodDto
{
    public int PaymentMethodId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
