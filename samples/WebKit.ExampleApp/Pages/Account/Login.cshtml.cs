using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebKit.Web.Navigation;

namespace WebKit.ExampleApp.Pages.Account;

public sealed class LoginModel : PageModel
{
    [BindProperty]
    public string UserName { get; set; } = string.Empty;

    [BindProperty]
    public bool IsAdmin { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            ModelState.AddModelError(nameof(UserName), "Enter a user name.");
            return Page();
        }

        List<Claim> claims = [new(ClaimTypes.Name, UserName.Trim()), new(ClaimTypes.NameIdentifier, UserName.Trim().ToLowerInvariant())];
        if (IsAdmin)
        {
            claims.Add(new(ClaimTypes.Role, "admin"));
            claims.Add(new("capability", "admin"));
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return LocalUrl.IsSafe(HttpContext, ReturnUrl) ? LocalRedirect(ReturnUrl!) : RedirectToPage("/Index");
    }
}
