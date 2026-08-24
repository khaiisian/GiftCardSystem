using System;
using System.Collections.Generic;

namespace GiftCardSystem.Database.AppDbContextModels;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? PhNumber { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte Role { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
