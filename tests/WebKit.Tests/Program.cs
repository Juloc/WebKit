using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebKit.Core.Errors;
using WebKit.Core.Pagination;
using WebKit.Core.Results;
using WebKit.Core.Validation;
using WebKit.ExampleApp.Features.Catalog;
using WebKit.UI;
using WebKit.Web.Configuration;
using WebKit.Web.Identity;
using WebKit.Web.Navigation;
using WebKit.Web.Notifications;
using WebKit.Web.Pagination;

namespace WebKit.Tests;

internal static class Program
{
    private static int Main()
    {
        List<(string Name, Action Test)> tests =
        [
            ("Result keeps success value and error state", ResultKeepsSuccessValue),
            ("Result rejects empty failures and hidden values", ResultRejectsInvalidFailures),
            ("Validation returns stable field errors", ValidationReturnsStableFieldErrors),
            ("Page request clamps untrusted values", PageRequestClampsValues),
            ("Page result calculates navigation", PageResultCalculatesNavigation),
            ("Pagination keeps arbitrary query parameters", PaginationKeepsQueryParameters),
            ("Error factories use stable codes", ErrorFactoriesUseStableCodes),
            ("Page request binder uses safe defaults", PageRequestBinderUsesSafeDefaults),
            ("Local URL rejects external redirects", LocalUrlRejectsExternalRedirects),
            ("Capability evaluator is shared by user context and policy", CapabilityEvaluatorIsShared),
            ("Flash kinds preserve all semantic states", FlashKindsPreserveSemanticStates),
            ("Catalog search supports empty results", CatalogSearchSupportsEmptyResults)
        ];

        int failures = 0;
        foreach ((string name, Action test) in tests)
        {
            try
            {
                test();
                Console.WriteLine($"PASS {name}");
            }
            catch (Exception exception)
            {
                failures++;
                Console.Error.WriteLine($"FAIL {name}: {exception.Message}");
            }
        }

        Console.WriteLine($"{tests.Count - failures}/{tests.Count} checks passed.");
        return failures == 0 ? 0 : 1;
    }

    private static void ResultKeepsSuccessValue()
    {
        Result<string> result = Result<string>.Success("ready");
        AssertEx.True(result.IsSuccess);
        AssertEx.Equal("ready", result.Value);
        AssertEx.Equal(string.Empty, result.Error.Code);
    }

    private static void PageRequestClampsValues()
    {
        PageRequest request = new(0, 500);
        AssertEx.Equal(1, request.Page);
        AssertEx.Equal(100, request.PageSize);
        AssertEx.Equal(0, request.Skip);
    }

    private static void ResultRejectsInvalidFailures()
    {
        Result<string> failed = Result<string>.Failure(WebKitError.NotFound());
        AssertEx.Throws<InvalidOperationException>(() => _ = failed.Value);
        AssertEx.Throws<ArgumentException>(() => Result<string>.Failure(WebKitError.None));
    }

    private static void ValidationReturnsStableFieldErrors()
    {
        ValidationResult validation = new();
        validation.Add("name", "Name is required.");
        AssertEx.False(validation.IsValid);
        AssertEx.Equal("name", validation.Errors[0].Field);
        AssertEx.Equal("Name is required.", validation.Errors[0].Message);
    }

    private static void PageResultCalculatesNavigation()
    {
        PageResult<int> result = new([1, 2], 2, 2, 5);
        AssertEx.Equal(3, result.PageCount);
        AssertEx.True(result.HasPreviousPage);
        AssertEx.True(result.HasNextPage);
    }

    private static void PaginationKeepsQueryParameters()
    {
        PaginationModel pagination = new(1, 3, "/Products", new Dictionary<string, string?>
        {
            ["q"] = "open items",
            ["status"] = "active",
            ["pageSize"] = "10"
        });
        string url = pagination.UrlFor(2);
        AssertEx.True(url.Contains("page=2", StringComparison.Ordinal));
        AssertEx.True(url.Contains("q=open%20items", StringComparison.Ordinal));
        AssertEx.True(url.Contains("status=active", StringComparison.Ordinal));
        AssertEx.True(url.Contains("pageSize=10", StringComparison.Ordinal));
    }

    private static void ErrorFactoriesUseStableCodes()
    {
        AssertEx.Equal("not_found", WebKitError.NotFound().Code);
        AssertEx.Equal("forbidden", WebKitError.Forbidden().Code);
    }

    private static void PageRequestBinderUsesSafeDefaults()
    {
        QueryCollection query = new(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { ["page"] = "-2", ["pageSize"] = "999" });
        PageRequest request = PageRequestBinder.From(query);
        AssertEx.Equal(1, request.Page);
        AssertEx.Equal(100, request.PageSize);
    }

    private static void LocalUrlRejectsExternalRedirects()
    {
        DefaultHttpContext context = new();
        context.Request.Path = "/account/login";
        AssertEx.True(LocalUrl.IsSafe(context, "/products"));
        AssertEx.False(LocalUrl.IsSafe(context, "https://example.test/phishing"));
        AssertEx.False(LocalUrl.IsSafe(context, "//example.test/phishing"));
        AssertEx.False(LocalUrl.IsSafe(context, "javascript:alert(1)"));
    }

    private static void CapabilityEvaluatorIsShared()
    {
        WebKitWebOptions options = new() { CapabilityClaimType = "permission", AdminRole = "administrator" };
        DefaultCapabilityEvaluator evaluator = new(Options.Create(options));
        ClaimsPrincipal capabilityUser = new(new ClaimsIdentity([new Claim("permission", "reports.read")], "test"));
        ClaimsPrincipal adminUser = new(new ClaimsIdentity([new Claim(ClaimTypes.Role, "administrator")], "test"));
        AssertEx.True(evaluator.HasCapability(capabilityUser, "reports.read"));
        AssertEx.True(evaluator.HasCapability(adminUser, "reports.read"));

        DefaultHttpContext context = new() { User = capabilityUser };
        HttpUserContext userContext = new(new HttpContextAccessor { HttpContext = context }, evaluator);
        AssertEx.True(userContext.HasCapability("reports.read"));

        AuthorizationHandlerContext authorization = new([new CapabilityRequirement("reports.read")], capabilityUser, null);
        new CapabilityAuthorizationHandler(evaluator).HandleAsync(authorization).GetAwaiter().GetResult();
        AssertEx.True(authorization.HasSucceeded);
    }

    private static void FlashKindsPreserveSemanticStates()
    {
        AssertEx.Equal(4, Enum.GetValues<FlashMessageKind>().Length);
        AssertEx.Equal(FlashMessageKind.Warning, new FlashMessage(FlashMessageKind.Warning, "Careful").Kind);
    }

    private static void CatalogSearchSupportsEmptyResults()
    {
        InMemoryProductCatalog catalog = new();
        PageResult<Product> page = catalog.Search("does-not-exist", new(1, 20));
        AssertEx.Equal(0, page.TotalCount);
        AssertEx.Equal(0, page.Items.Count);
    }
}

internal static class AssertEx
{
    public static void True(bool condition)
    {
        if (!condition)
        {
            throw new InvalidOperationException("Expected true.");
        }
    }

    public static void False(bool condition) => True(!condition);

    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
        }
    }

    public static void Throws<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }
}
