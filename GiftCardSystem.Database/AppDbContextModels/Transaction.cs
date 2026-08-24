using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class Transaction
{
    public long TransactionId { get; set; }

    public string TransactionNo { get; set; } = null!;

    public int UserId { get; set; }

    public int PaymentMethodId { get; set; }

    public int GiftCardId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public byte TypeOfBuying { get; set; }

    public string? RecipientName { get; set; }

    public string? RecipientPhone { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual GiftCard GiftCard { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    public virtual User User { get; set; } = null!;
}
