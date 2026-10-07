namespace WebKit.Web.Configuration;

public sealed class WebKitWebOptions
{
    public string CapabilityClaimType { get; set; } = "capability";

    public string AdminRole { get; set; } = "admin";

    public bool AdminBypassEnabled { get; set; } = true;

    public string ReturnUrlParameter { get; set; } = "returnUrl";
}
