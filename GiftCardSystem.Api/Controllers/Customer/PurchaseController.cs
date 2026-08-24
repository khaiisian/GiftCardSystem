using GiftCardSystem.Api.Helper;
using GiftCardSystem.Service.Purchase;
using GiftCardSystem.Service.Purchase.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers.Customer;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PurchaseController : ControllerBase
{
    private readonly PurchaseService _service;
    private readonly HttpContextService _httpContextService;

    public PurchaseController(PurchaseService service, HttpContextService httpContextService)
    {
        _service = service;
        _httpContextService = httpContextService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutRequestDto dto)
    {
        var result = await _service.CheckoutAsync(dto);
        return result is null ? BadRequest("Unable to checkout this gift card / payment method.") : Ok(result);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay(PayRequestDto dto)
    {
        var buyerId = _httpContextService.UserId();
        if (buyerId is null) return Unauthorized();

        var result = await _service.PayAsync(buyerId.Value, dto);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("verify/{promoCode}")]
    public async Task<IActionResult> Verify(string promoCode)
    {
        var result = await _service.VerifyPromoAsync(promoCode);
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var buyerId = _httpContextService.UserId();
        if (buyerId is null) return Unauthorized();

        return Ok(await _service.GetHistoryAsync(buyerId.Value));
    }

    [HttpGet("tickets/unused")]
    public async Task<IActionResult> Unused()
    {
        var buyerId = _httpContextService.UserId();
        if (buyerId is null) return Unauthorized();

        return Ok(await _service.GetOwnedTicketsAsync(buyerId.Value, 1));
    }

    [HttpGet("tickets/used")]
    public async Task<IActionResult> Used()
    {
        var buyerId = _httpContextService.UserId();
        if (buyerId is null) return Unauthorized();

        return Ok(await _service.GetOwnedTicketsAsync(buyerId.Value, 2));
    }
}
