using Microsoft.AspNetCore.Http;

namespace WebKit.Web.Navigation;

public static class LocalUrl
{
    public static bool IsSafe(HttpContext httpContext, string? value)
    {
        string basePath = httpContext.Request.PathBase.ToString();
        return !string.IsNullOrWhiteSpace(value) && value.StartsWith('/') && !value.StartsWith("//", StringComparison.Ordinal) && Uri.TryCreate(value, UriKind.Relative, out _) && value.StartsWith(basePath, StringComparison.OrdinalIgnoreCase);
    }

    public static string OrDefault(HttpContext httpContext, string? value, string fallback)
    {
        return IsSafe(httpContext, value) ? value! : fallback;
    }
}
