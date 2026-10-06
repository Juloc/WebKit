using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.ExampleApp.Features.Catalog;

namespace WebKit.ExampleApp.Pages.Products;

[Authorize]
public sealed class ProductDetailsModel(IProductCatalog catalog) : PageModel
{
    public Product? Product { get; private set; }

    public IActionResult OnGet(Guid id)
    {
        WebKit.Core.Results.Result<Product> result = catalog.Find(id);
        if (result.IsFailure)
        {
            return NotFound();
        }

        Product = result.Value;
        return Page();
    }

    public IActionResult OnPostDelete(Guid id)
    {
        return RedirectToPage("/Products/Details", new { id });
    }
}
