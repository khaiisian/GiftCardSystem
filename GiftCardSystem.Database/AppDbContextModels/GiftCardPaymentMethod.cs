using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class GiftCardPaymentMethod
{
    public int GiftCardPaymentMethodId { get; set; }

    public int GiftCardId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Discount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual GiftCard GiftCard { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
