using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebKit.ExampleApp.Pages.Admin;

[Authorize(Policy = "admin")]
public sealed class AdminIndexModel : PageModel
{
    public void OnGet()
    {
    }
}
