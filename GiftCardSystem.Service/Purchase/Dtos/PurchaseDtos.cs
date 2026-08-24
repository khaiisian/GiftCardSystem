namespace GiftCardSystem.Service.Purchase.Dtos;

public class CheckoutRequestDto
{
    public int GiftCardId { get; set; }
    public int PaymentMethodId { get; set; }
    public int Quantity { get; set; }
}

public class CheckoutResultDto
{
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public decimal DiscountPercent { get; set; }
}

public class PayRequestDto
{
    public int GiftCardId { get; set; }
    public int PaymentMethodId { get; set; }
    public int Quantity { get; set; }
    public byte TypeOfBuying { get; set; }
    public string? RecipientName { get; set; }
    public string? RecipientPhone { get; set; }
}

public class PurchaseResultDto
{
    public string TransactionNo { get; set; } = null!;
    public decimal Total { get; set; }
    public List<string> PromoCodes { get; set; } = new();
}

public class PayResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public PurchaseResultDto? Data { get; set; }
}

public class VerifyResultDto
{
    public bool Valid { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class PurchaseHistoryDto
{
    public string TransactionNo { get; set; } = null!;
    public DateTime TransactionDate { get; set; }
    public string GiftCardTitle { get; set; } = null!;
    public decimal Total { get; set; }
    public byte TypeOfBuying { get; set; }
    public List<string> PromoCodes { get; set; } = new();
}

public class OwnedTicketDto
{
    public string PromoCode { get; set; } = null!;
    public string GiftCardTitle { get; set; } = null!;
    public byte Status { get; set; }
    public string? QrPath { get; set; }
}
