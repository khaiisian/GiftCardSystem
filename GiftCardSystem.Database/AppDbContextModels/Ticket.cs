using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class Ticket
{
    public long TicketId { get; set; }

    public int GiftCardId { get; set; }

    public string PromoCode { get; set; } = null!;

    public string? QrPath { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual GiftCard GiftCard { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
