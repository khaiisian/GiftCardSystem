using GiftCardSystem.Service.PaymentMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers.Customer;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentMethodController : ControllerBase
{
    private readonly PaymentMethodService _service;

    public PaymentMethodController(PaymentMethodService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var methods = await _service.GetAllAsync();
        return Ok(methods);
    }
}
