using Microsoft.AspNetCore.Http;
using WebKit.Core.Pagination;

namespace WebKit.Web.Pagination;

public static class PageRequestBinder
{
    public static PageRequest From(IQueryCollection query, int defaultPageSize = 20)
    {
        int page = ParsePositiveInt(query["page"], 1);
        int pageSize = ParsePositiveInt(query["pageSize"], defaultPageSize);
        return new PageRequest(page, Math.Clamp(pageSize, 1, 100));
    }

    private static int ParsePositiveInt(string? value, int fallback) => int.TryParse(value, out int parsed) && parsed > 0 ? parsed : fallback;
}
