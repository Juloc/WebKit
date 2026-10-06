using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.Core.Validation;
using WebKit.ExampleApp.Features.Catalog;
using WebKit.Web.Notifications;
using WebKit.Web.Validation;

namespace WebKit.ExampleApp.Pages.Products;

[Authorize]
public sealed class ProductEditModel(IProductCatalog catalog, IFlashMessageStore flashMessages) : PageModel
{
    [BindProperty]
    public ProductDraft Input { get; set; } = new(null, string.Empty, string.Empty, "Active");

    public bool IsEdit => Input.Id.HasValue;

    public IActionResult OnGet(Guid? id)
    {
        if (id is null)
        {
            return Page();
        }

        WebKit.Core.Results.Result<Product> result = catalog.Find(id.Value);
        if (result.IsFailure)
        {
            return NotFound();
        }

        Product product = result.Value;
        Input = new ProductDraft(product.Id, product.Name, product.Category, product.Status);
        return Page();
    }

    public IActionResult OnPost()
    {
        ValidationResult validation = Input.Validate();
        if (!validation.IsValid)
        {
            ModelState.AddErrors(validation);
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        WebKit.Core.Results.Result<Product> result = catalog.Save(Input);
        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error.Message);
            return Page();
        }

        flashMessages.Add(new FlashMessage(FlashMessageKind.Success, $"{result.Value.Name} saved."));
        return RedirectToPage("/Products/Details", new { id = result.Value.Id });
    }
}
