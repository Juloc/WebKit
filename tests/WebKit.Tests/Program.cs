using Microsoft.AspNetCore.Http;
using WebKit.Core.Errors;
using WebKit.Core.Pagination;
using WebKit.Core.Results;
using WebKit.ExampleApp.Features.Catalog;
using WebKit.Web.Navigation;
using WebKit.Web.Pagination;

namespace WebKit.Tests;

internal static class Program
{
    private static int Main()
    {
        List<(string Name, Action Test)> tests =
        [
            ("Result keeps success value and error state", ResultKeepsSuccessValue),
            ("Page request clamps untrusted values", PageRequestClampsValues),
            ("Page result calculates navigation", PageResultCalculatesNavigation),
            ("Error factories use stable codes", ErrorFactoriesUseStableCodes),
            ("Page request binder uses safe defaults", PageRequestBinderUsesSafeDefaults),
            ("Local URL rejects external redirects", LocalUrlRejectsExternalRedirects),
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

    private static void PageResultCalculatesNavigation()
    {
        PageResult<int> result = new([1, 2], 2, 2, 5);
        AssertEx.Equal(3, result.PageCount);
        AssertEx.True(result.HasPreviousPage);
        AssertEx.True(result.HasNextPage);
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
}
