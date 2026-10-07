using Microsoft.AspNetCore.Authorization;

namespace WebKit.Web.Identity;

public sealed class CapabilityRequirement(string capability) : IAuthorizationRequirement
{
    public string Capability { get; } = capability;
}

public sealed class CapabilityAuthorizationHandler(ICapabilityEvaluator evaluator) : AuthorizationHandler<CapabilityRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CapabilityRequirement requirement)
    {
        if (evaluator.HasCapability(context.User, requirement.Capability))
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
