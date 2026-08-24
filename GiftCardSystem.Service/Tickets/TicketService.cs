using GiftCardSystem.Database.AppDbContextModels;
using GiftCardSystem.Repository.GiftCards;
using GiftCardSystem.Repository.Tickets;
using QRCoder;

namespace GiftCardSystem.Service.Tickets;

public class TicketService
{
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private readonly ITicketRepository _ticketRepo;
    private readonly IGiftCardRepository _giftCardRepo;

    public TicketService(ITicketRepository ticketRepo, IGiftCardRepository giftCardRepo)
    {
        _ticketRepo = ticketRepo;
        _giftCardRepo = giftCardRepo;
    }

    // Returns number of tickets generated, or null if the gift card doesn't exist.
    public async Task<int?> GenerateTicketsAsync(int giftCardId, int count)
    {
        var giftCard = await _giftCardRepo.GetByIdAsync(giftCardId);
        if (giftCard is null) return null;

        var existing = await _ticketRepo.GetAllPromoCodesAsync();
        var batch = new HashSet<string>();
        var tickets = new List<Ticket>();

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes");
        Directory.CreateDirectory(folder);

        while (tickets.Count < count)
        {
            var code = GeneratePromoCode();

            // skip if the code already exists in the DB or was already made in this batch
            if (existing.Contains(code) || !batch.Add(code))
                continue;

            var fileName = $"{code}.png";
            await File.WriteAllBytesAsync(Path.Combine(folder, fileName), GenerateQr(code));

            tickets.Add(new Ticket
            {
                GiftCardId = giftCardId,
                PromoCode = code,
                QrPath = $"/qrcodes/{fileName}",
                Status = 0,   // Available
                CreatedAt = DateTime.UtcNow
            });
        }

        await _ticketRepo.AddTicketsAsync(tickets);
        return tickets.Count;
    }

    // 6 digits + 5 letters = 11 chars, e.g. "483920ABCDE"
    private string GeneratePromoCode()
    {
        var digits = Random.Shared.Next(0, 1_000_000).ToString("D6");

        var letters = new char[5];
        for (int i = 0; i < 5; i++)
            letters[i] = Letters[Random.Shared.Next(Letters.Length)];

        return digits + new string(letters);
    }

    private byte[] GenerateQr(string text)
    {
        using var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qr = new PngByteQRCode(data);
        return qr.GetGraphic(20);
    }
}
