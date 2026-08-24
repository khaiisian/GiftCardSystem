using GiftCardSystem.Service.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class TicketController : ControllerBase
{
    private readonly TicketService _service;

    public TicketController(TicketService service)
    {
        _service = service;
    }

    [HttpPost("generate/{giftCardId}")]
    public async Task<IActionResult> Generate(int giftCardId, [FromQuery] int count = 1000)
    {
        var result = await _service.GenerateTicketsAsync(giftCardId, count);
        return result is null
            ? NotFound("Gift card not found.")
            : Ok($"{result} tickets generated.");
    }
}
