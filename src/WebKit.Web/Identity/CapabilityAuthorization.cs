using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebKit.Web.Configuration;

namespace WebKit.Web.Identity;

public sealed class CapabilityRequirement(string capability) : IAuthorizationRequirement
{
    public string Capability { get; } = capability;
}

public sealed class CapabilityAuthorizationHandler(IOptions<WebKitWebOptions> options) : AuthorizationHandler<CapabilityRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CapabilityRequirement requirement)
    {
        bool hasCapability = context.User.Claims.Any(claim => claim.Type == options.Value.CapabilityClaimType && string.Equals(claim.Value, requirement.Capability, StringComparison.OrdinalIgnoreCase));
        bool isAdmin = context.User.IsInRole("admin");
        if (hasCapability || isAdmin)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public static class CapabilityPolicyExtensions
{
    public static AuthorizationPolicyBuilder RequireCapability(this AuthorizationPolicyBuilder builder, string capability)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(capability);
        return builder.AddRequirements(new CapabilityRequirement(capability));
    }
}
