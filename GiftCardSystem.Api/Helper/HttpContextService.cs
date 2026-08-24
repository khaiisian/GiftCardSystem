using System.Security.Claims;

namespace GiftCardSystem.Api.Helper;

public class HttpContextService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? UserEmail() => _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

    public int? UserId()
    {
        var sub = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? _contextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
        return int.TryParse(sub, out var id) ? id : null;
    }
}


