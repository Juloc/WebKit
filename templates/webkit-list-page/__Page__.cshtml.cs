using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.UI;

namespace __Page__;

public sealed class __Page__Model : PageModel
{
    public string? Search { get; private set; }

    public bool HasResults { get; private set; }

    public PaginationModel Pagination => new(1, 1, "./__Page__", new Dictionary<string, string?> { ["q"] = Search });

    public void OnGet(string? q)
    {
        Search = q;
    }
}
