using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.Core.Validation;
using WebKit.Web.Notifications;

namespace WebKit.ExampleApp.Pages.Settings;

[Authorize]
public sealed class IndexModel(IFlashMessageStore flashMessages) : PageModel
{
    [BindProperty]
    public string Density { get; set; } = "Comfortable";

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Density))
        {
            ModelState.AddModelError(nameof(Density), "Choose a density.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        flashMessages.Add(new FlashMessage(FlashMessageKind.Success, "Settings saved."));
        return RedirectToPage();
    }
}
