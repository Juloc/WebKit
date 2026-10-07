using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace WebKit.Web.Navigation;

public static class LocalUrl
{
    public static bool IsSafe(HttpContext httpContext, string? value)
    {
        ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor());
        UrlHelper urlHelper = new(actionContext);
        string basePath = httpContext.Request.PathBase.ToString();
        return !string.IsNullOrWhiteSpace(value)
            && urlHelper.IsLocalUrl(value)
            && (string.IsNullOrEmpty(basePath) || value.StartsWith(basePath, StringComparison.OrdinalIgnoreCase));
    }

    public static string OrDefault(HttpContext httpContext, string? value, string fallback)
    {
        return IsSafe(httpContext, value) ? value! : fallback;
    }
}
