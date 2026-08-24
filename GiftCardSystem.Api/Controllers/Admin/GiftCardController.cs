using GiftCardSystem.Service.GiftCards;
using GiftCardSystem.Service.GiftCards.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardSystem.Api.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class GiftCardController : ControllerBase
{
    private readonly GiftCardService _service;

    public GiftCardController(GiftCardService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGiftCardDtos dto)
    {
        var result = await _service.CreateGiftCardAsync(dto);
        return result
            ? Ok("Gift card created successfully.")
            : StatusCode(500, "Failed to create gift card.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, EditGiftCardDto dto)
    {
        var result = await _service.EditGiftCardAsync(id, dto);
        return result switch
        {
            GiftCardResult.Success  => Ok("Gift card updated successfully."),
            GiftCardResult.NotFound => NotFound("Gift card not found."),
            _                       => StatusCode(500, "Failed to update gift card.")
        };
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result = await _service.DeactivateGiftCardAsync(id);
        return result switch
        {
            GiftCardResult.Success  => Ok("Gift card deactivated."),
            GiftCardResult.NotFound => NotFound("Gift card not found."),
            _                       => StatusCode(500, "Failed to deactivate gift card.")
        };
    }
}
