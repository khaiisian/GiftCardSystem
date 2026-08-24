using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<GiftCardPaymentMethod> GiftCardPaymentMethods { get; set; } = new List<GiftCardPaymentMethod>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
