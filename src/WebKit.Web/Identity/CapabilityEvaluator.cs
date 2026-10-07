using System.Security.Claims;
using Microsoft.Extensions.Options;
using WebKit.Web.Configuration;

namespace WebKit.Web.Identity;

public interface ICapabilityEvaluator
{
    bool HasCapability(ClaimsPrincipal user, string capability);

    bool IsAdministrator(ClaimsPrincipal user);
}

public sealed class DefaultCapabilityEvaluator(IOptions<WebKitWebOptions> options) : ICapabilityEvaluator
{
    public bool HasCapability(ClaimsPrincipal user, string capability)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(capability);

        WebKitWebOptions settings = options.Value;
        return (settings.AdminBypassEnabled && IsAdministrator(user))
            || user.Claims.Any(claim => claim.Type == settings.CapabilityClaimType && string.Equals(claim.Value, capability, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsAdministrator(ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.IsInRole(options.Value.AdminRole);
    }
}
