using GiftCardSystem.Service.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers.Customer;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly StoreService _service;

    public StoreController(StoreService service)
    {
        _service = service;
    } 

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var list = await _service.GetListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(int id)
    {
        var detail = await _service.GetDetailAsync(id);
        return detail is null ? NotFound("Gift card not found.") : Ok(detail);
    }
}
