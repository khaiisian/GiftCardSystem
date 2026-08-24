using GiftCardSystem.Database.AppDbContextModels;
using GiftCardSystem.Repository.GiftCards;
using GiftCardSystem.Repository.Tickets;
using GiftCardSystem.Repository.Transactions;
using GiftCardSystem.Service.Purchase.Dtos;

namespace GiftCardSystem.Service.Purchase;

public class PurchaseService
{
    private readonly IGiftCardRepository _giftCardRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly ITransactionRepository _txnRepo;

    public PurchaseService(
        IGiftCardRepository giftCardRepo,
        ITicketRepository ticketRepo,
        ITransactionRepository txnRepo)
    {
        _giftCardRepo = giftCardRepo;
        _ticketRepo = ticketRepo;
        _txnRepo = txnRepo;
    }

    public async Task<CheckoutResultDto?> CheckoutAsync(CheckoutRequestDto req)
    {
        var card = await _giftCardRepo.GetByIdWithPaymentMethodsAsync(req.GiftCardId);
        if (card is null || card.Status != 1) return null;

        var discount = GetDiscount(card, req.PaymentMethodId, out var error);
        if (error is not null) return null;

        var unitPrice = card.Amount - (card.Amount * discount / 100m);

        return new CheckoutResultDto
        {
            UnitPrice = unitPrice,
            Total = unitPrice * req.Quantity,
            DiscountPercent = discount
        };
    }

    public async Task<PayResult> PayAsync(int buyerId, PayRequestDto req)
    {
        var card = await _giftCardRepo.GetByIdWithPaymentMethodsAsync(req.GiftCardId);
        if (card is null) return Fail("Gift card not found.");
        if (card.Status != 1) return Fail("Gift card is not active.");
        if (card.ExpiryDate < DateTime.UtcNow) return Fail("Gift card has expired.");
        if (req.Quantity < 1) return Fail("Quantity must be at least 1.");

        var discount = GetDiscount(card, req.PaymentMethodId, out var discountError);
        if (discountError is not null) return Fail(discountError);

        var unitPrice = card.Amount - (card.Amount * discount / 100m);
        var total = unitPrice * req.Quantity;

        if (req.TypeOfBuying == 0)
        {
            if (card.MaxPurchaseLimit is int max)
            {
                var bought = await _txnRepo.CountSelfPurchasedAsync(buyerId, card.GiftCardId);
                if (bought + req.Quantity > max) return Fail("Purchase limit exceeded.");
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(req.RecipientPhone))
                return Fail("Recipient phone is required for a gift.");

            if (card.MaxPurchaseLimit is int max)
            {
                var gifted = await _txnRepo.CountGiftedByBuyerAsync(buyerId, card.GiftCardId);
                if (gifted + req.Quantity > max) return Fail("Gift purchase limit exceeded.");
            }

            if (card.GiftPerUserLimit is int perUser)
            {
                var toRecipient = await _txnRepo.CountGiftedToRecipientAsync(req.RecipientPhone, card.GiftCardId);
                if (toRecipient + req.Quantity > perUser) return Fail("Gift-per-recipient limit exceeded.");
            }
        }

        var tickets = await _ticketRepo.GetAvailableTicketsAsync(card.GiftCardId, req.Quantity);
        if (tickets.Count < req.Quantity) return Fail("Not enough tickets available.");

        foreach (var t in tickets) t.Status = 1;

        var txn = new Transaction
        {
            TransactionNo = $"TXN-{Guid.NewGuid():N}"[..24],
            UserId = buyerId,
            PaymentMethodId = req.PaymentMethodId,
            GiftCardId = card.GiftCardId,
            TotalAmount = total,
            TransactionDate = DateTime.UtcNow,
            TypeOfBuying = req.TypeOfBuying,
            RecipientName = req.TypeOfBuying == 1 ? req.RecipientName : null,
            RecipientPhone = req.TypeOfBuying == 1 ? req.RecipientPhone : null,
            CreatedAt = DateTime.UtcNow,
            TransactionDetails = tickets.Select(t => new TransactionDetail
            {
                TicketId = t.TicketId,
                UnitPrice = unitPrice,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        var saved = await _txnRepo.AddTransactionAsync(txn);
        if (!saved) return Fail("Failed to complete the purchase.");

        return new PayResult
        {
            Success = true,
            Data = new PurchaseResultDto
            {
                TransactionNo = txn.TransactionNo,
                Total = total,
                PromoCodes = tickets.Select(t => t.PromoCode).ToList()
            }
        };
    }

    private decimal GetDiscount(GiftCard card, int paymentMethodId, out string? error)
    {
        error = null;
        var gcpm = card.GiftCardPaymentMethods.FirstOrDefault(x => x.PaymentMethodId == paymentMethodId);
        if (gcpm is null)
        {
            error = "Payment method not accepted for this card.";
            return 0m;
        }

        return gcpm.PaymentMethod.Name == "KBZ Pay" ? 5m : gcpm.Discount;
    }

    private PayResult Fail(string message) => new() { Success = false, Error = message };

    public async Task<VerifyResultDto> VerifyPromoAsync(string promoCode)
    {
        var ticket = await _ticketRepo.GetByPromoCodeAsync(promoCode);

        if (ticket is null)
            return new VerifyResultDto { Valid = false, Status = "NotFound", Message = "Code not found." };

        if (ticket.Status == 0)
            return new VerifyResultDto { Valid = false, Status = "Available", Message = "This code has not been sold yet." };

        if (ticket.Status == 2)
            return new VerifyResultDto { Valid = false, Status = "Used", Message = "This code has already been used." };

        if (ticket.GiftCard.ExpiryDate < DateTime.UtcNow)
            return new VerifyResultDto { Valid = false, Status = "Expired", Message = "This gift card has expired." };

        return new VerifyResultDto { Valid = true, Status = "Unused", Message = "Valid." };
    }

    public async Task<List<PurchaseHistoryDto>> GetHistoryAsync(int userId)
    {
        var transactions = await _txnRepo.GetByUserAsync(userId);

        return transactions.Select(t => new PurchaseHistoryDto
        {
            TransactionNo = t.TransactionNo,
            TransactionDate = t.TransactionDate,
            GiftCardTitle = t.GiftCard.Title,
            Total = t.TotalAmount,
            TypeOfBuying = t.TypeOfBuying,
            PromoCodes = t.TransactionDetails.Select(td => td.Ticket.PromoCode).ToList()
        }).ToList();
    }

    public async Task<List<OwnedTicketDto>> GetOwnedTicketsAsync(int userId, byte status)
    {
        var tickets = await _ticketRepo.GetOwnedTicketsAsync(userId, status);

        return tickets.Select(t => new OwnedTicketDto
        {
            PromoCode = t.PromoCode,
            GiftCardTitle = t.GiftCard.Title,
            Status = t.Status,
            QrPath = t.QrPath
        }).ToList();
    }
}
