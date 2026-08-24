using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class GiftCard
{
    public int GiftCardId { get; set; }

    public string GiftCardNo { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime ExpiryDate { get; set; }

    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    public byte Status { get; set; }

    public int? MaxPurchaseLimit { get; set; }

    public int? GiftPerUserLimit { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<GiftCardPaymentMethod> GiftCardPaymentMethods { get; set; } = new List<GiftCardPaymentMethod>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
