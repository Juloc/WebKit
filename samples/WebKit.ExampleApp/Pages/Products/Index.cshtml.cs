using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.Core.Pagination;
using WebKit.ExampleApp.Features.Catalog;
using WebKit.UI;
using WebKit.Web.Pagination;

namespace WebKit.ExampleApp.Pages.Products;

[Authorize]
public sealed class ProductsIndexModel(IProductCatalog catalog) : PageModel
{
    public string? Search { get; private set; }

    public PageResult<Product> Results { get; private set; } = new([], 1, 20, 0);

    public PaginationModel Pagination => new(Results.Page, Results.PageCount, "/Products", Search);

    public void OnGet(string? q)
    {
        Search = q;
        Results = catalog.Search(q, PageRequestBinder.From(Request.Query));
    }
}
