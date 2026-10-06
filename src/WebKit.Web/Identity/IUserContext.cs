using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebKit.Web.Identity;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    string? UserId { get; }

    string? DisplayName { get; }

    bool HasCapability(string capability);
}

public sealed class HttpUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private ClaimsPrincipal User => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());

    public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;

    public string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? DisplayName => User.Identity?.Name;

    public bool HasCapability(string capability) => User.IsInRole(capability) || User.Claims.Any(claim => claim.Type == "capability" && string.Equals(claim.Value, capability, StringComparison.OrdinalIgnoreCase));
}
