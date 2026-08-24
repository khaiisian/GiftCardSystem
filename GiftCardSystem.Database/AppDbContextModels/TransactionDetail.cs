using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class TransactionDetail
{
    public long TransactionDetailId { get; set; }

    public long TransactionId { get; set; }

    public long TicketId { get; set; }

    public decimal? UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
